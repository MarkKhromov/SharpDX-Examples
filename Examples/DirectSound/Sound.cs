using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using SharpDX.DirectSound;
using SharpDX.Multimedia;

namespace SharpDXExamples.Examples.DirectSound {
    public class Sound {
        struct WaveHeaderType {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] ChunkId;
            public ulong ChunkSize;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] Format;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] SubChunkId;
            public ulong SubChunkSize;
            public ushort AudioFormat;
            public ushort NumChannels;
            public ulong SampleRate;
            public ulong BytesPerSecond;
            public ushort BlockAlign;
            public ushort BitsPerSample;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] DataChunkId;
            public ulong DataSize;
        }

        SharpDX.DirectSound.DirectSound directSound;
        PrimarySoundBuffer primaryBuffer;
        SecondarySoundBuffer secondaryBuffer;

        public bool Initialize(IntPtr hwnd) {
            if(!InitializeDirectSound(hwnd))
                return false;

            if(!LoadWaveFile(@"Examples\DirectSound\Data\Sound.wav"))
                return false;

            if(!PlayWaveFile())
                return false;

            return true;
        }

        public void Shutdown() {
            ShutdownWaveFile();
            ShutdownDirectSound();
        }

        bool InitializeDirectSound(IntPtr hwnd) {
            try {
                directSound = new SharpDX.DirectSound.DirectSound();
                directSound.SetCooperativeLevel(hwnd, CooperativeLevel.Priority);

                var soundBufferDescription = new SoundBufferDescription {
                    Flags = BufferFlags.PrimaryBuffer | BufferFlags.ControlVolume,
                    BufferBytes = 0,
                    Format = null,
                    AlgorithmFor3D = Guid.Empty
                };

                primaryBuffer = new PrimarySoundBuffer(directSound, soundBufferDescription);

                var samplesPerSec = 44100;
                var bitsPerSample = 16;
                var nChannels = 2;
                var blockAlign = bitsPerSample / 8 * nChannels;
                var nAvgBytesPerSec = samplesPerSec * blockAlign;
                var waveFormat = WaveFormat.CreateCustomFormat(
                    WaveFormatEncoding.Pcm,
                    samplesPerSec,
                    nChannels,
                    nAvgBytesPerSec,
                    blockAlign,
                    bitsPerSample
                );

                primaryBuffer.Format = waveFormat;
            } catch { return false; }
            return true;
        }

        void ShutdownDirectSound() {
            primaryBuffer.Dispose();
            directSound.Dispose();
        }

        bool LoadWaveFile(string fileName) {
            try {
                using(var streamReader = File.OpenRead(fileName)) {
                    var tempByteArray = new byte[4];

                    streamReader.Read(tempByteArray, 0, 4);
                    var chunkId = tempByteArray.Select(x => (char)x).ToArray();
                    streamReader.Read(tempByteArray, 0, 4);
                    var chunkSize = BitConverter.ToUInt32(tempByteArray, 0);
                    streamReader.Read(tempByteArray, 0, 4);
                    var format = tempByteArray.Select(x => (char)x).ToArray();
                    streamReader.Read(tempByteArray, 0, 4);
                    var subChunkId = tempByteArray.Select(x => (char)x).ToArray();
                    streamReader.Read(tempByteArray, 0, 4);
                    var subChunkSize = BitConverter.ToUInt32(tempByteArray, 0);
                    streamReader.Read(tempByteArray, 0, 2);
                    var audioFormat = BitConverter.ToUInt16(tempByteArray, 0);
                    streamReader.Read(tempByteArray, 0, 2);
                    var numChannels = BitConverter.ToUInt16(tempByteArray, 0);
                    streamReader.Read(tempByteArray, 0, 4);
                    var sampleRate = BitConverter.ToUInt32(tempByteArray, 0);
                    streamReader.Read(tempByteArray, 0, 4);
                    var bytesPerSecond = BitConverter.ToUInt32(tempByteArray, 0);
                    streamReader.Read(tempByteArray, 0, 2);
                    var blockAlign = BitConverter.ToUInt16(tempByteArray, 0);
                    streamReader.Read(tempByteArray, 0, 2);
                    var bitsPerSample = BitConverter.ToUInt16(tempByteArray, 0);
                    streamReader.Read(tempByteArray, 0, 4);
                    var dataChunkId = tempByteArray.Select(x => (char)x).ToArray();
                    streamReader.Read(tempByteArray, 0, 4);
                    var dataSize = BitConverter.ToUInt32(tempByteArray, 0);

                    var waveFileHeader = new WaveHeaderType {
                        ChunkId = chunkId,
                        ChunkSize = chunkSize,
                        Format = format,
                        SubChunkId = subChunkId,
                        SubChunkSize = subChunkSize,
                        AudioFormat = audioFormat,
                        NumChannels = numChannels,
                        SampleRate = sampleRate,
                        BytesPerSecond = bytesPerSecond,
                        BlockAlign = blockAlign,
                        BitsPerSample = bitsPerSample,
                        DataChunkId = dataChunkId,
                        DataSize = dataSize
                    };

                    if(
                        waveFileHeader.ChunkId[0] != 'R' ||
                        waveFileHeader.ChunkId[1] != 'I' ||
                        waveFileHeader.ChunkId[2] != 'F' ||
                        waveFileHeader.ChunkId[3] != 'F'
                    ) {
                        return false;
                    }

                    if(
                        waveFileHeader.Format[0] != 'W' ||
                        waveFileHeader.Format[1] != 'A' ||
                        waveFileHeader.Format[2] != 'V' ||
                        waveFileHeader.Format[3] != 'E'
                    ) {
                        return false;
                    }

                    if(
                        waveFileHeader.SubChunkId[0] != 'f' ||
                        waveFileHeader.SubChunkId[1] != 'm' ||
                        waveFileHeader.SubChunkId[2] != 't' ||
                        waveFileHeader.SubChunkId[3] != ' '
                    ) {
                        return false;
                    }

                    if(waveFileHeader.AudioFormat != (ushort)WaveFormatEncoding.Pcm)
                        return false;

                    if(waveFileHeader.NumChannels != 2)
                        return false;

                    if(waveFileHeader.SampleRate != 44100)
                        return false;

                    if(waveFileHeader.BitsPerSample != 16)
                        return false;

                    if(
                        waveFileHeader.DataChunkId[0] != 'd' ||
                        waveFileHeader.DataChunkId[1] != 'a' ||
                        waveFileHeader.DataChunkId[2] != 't' ||
                        waveFileHeader.DataChunkId[3] != 'a'
                    ) {
                        return false;
                    }

                    var waveFormat = WaveFormat.CreateCustomFormat(
                        WaveFormatEncoding.Pcm,
                        44100,
                        2,
                        4 * 44100,
                        4,
                        16
                    );

                    var soundBufferDescription = new SoundBufferDescription {
                        Flags = BufferFlags.ControlVolume,
                        BufferBytes = (int)waveFileHeader.DataSize,
                        Format = waveFormat,
                        AlgorithmFor3D = Guid.Empty
                    };

                    secondaryBuffer = new SecondarySoundBuffer(directSound, soundBufferDescription);

                    var waveData = new byte[waveFileHeader.DataSize];
                    streamReader.Read(waveData, 0, (int)waveFileHeader.DataSize);
                    secondaryBuffer.Write(waveData, 0, LockFlags.None);
                }
            } catch { return false; }
            return true;
        }

        void ShutdownWaveFile() {
            secondaryBuffer.Dispose();
        }

        bool PlayWaveFile() {
            try {
                secondaryBuffer.CurrentPosition = 0;
                secondaryBuffer.Volume = Volume.Maximum;
                secondaryBuffer.Play(0, PlayFlags.None);
            } catch { return false; }
            return true;
        }
    }
}