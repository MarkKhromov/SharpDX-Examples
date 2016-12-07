using System;
using SharpDX;

namespace SharpDXExamples.Examples.FrustumCulling {
    public class ModelList {
        struct ModelInfoType {
            public Color4 Color;
            public float PositionX;
            public float PositionY;
            public float PositionZ;
        }

        public int Count { get; private set; }
        ModelInfoType[] modelInfoList;

        public bool Initialize(int numModels) {
            try {
                Count = numModels;
                modelInfoList = new ModelInfoType[numModels];

                var random = new Random();
                for(int i = 0; i < Count; i++) {
                    var color = random.NextColor();
                    modelInfoList[i].Color = color;
                    var floatRand1 = random.NextFloat(float.MinValue, float.MaxValue);
                    var floatRand2 = random.NextFloat(float.MinValue, float.MaxValue);
                    modelInfoList[i].PositionX = (floatRand1 - floatRand2) / float.MaxValue * 10.0f;
                    floatRand1 = random.NextFloat(float.MinValue, float.MaxValue);
                    floatRand2 = random.NextFloat(float.MinValue, float.MaxValue);
                    modelInfoList[i].PositionY = (floatRand1 - floatRand2) / float.MaxValue * 10.0f;
                    floatRand1 = random.NextFloat(float.MinValue, float.MaxValue);
                    floatRand2 = random.NextFloat(float.MinValue, float.MaxValue);
                    modelInfoList[i].PositionZ = (floatRand1 - floatRand2) / float.MaxValue * 10.0f + 5.0f;
                }
            } catch { return false; }
            return true;
        }

        public void GetData(int index, out float positionX, out float positionY, out float positionZ, out Color4 color) {
            positionX = modelInfoList[index].PositionX;
            positionY = modelInfoList[index].PositionY;
            positionZ = modelInfoList[index].PositionZ;
            color = modelInfoList[index].Color;
        }
    }
}