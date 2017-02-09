cbuffer PerFrameBuffer {
	matrix World;
	matrix View;
	matrix Projection;
};

cbuffer FogBuffer {
	float FogStart;
	float FogEnd;
};

struct VertexInputType {
	float4 position : POSITION;
	float2 tex : TEXCOORD0;
};

struct PixelInputType {
	float4 position : SV_POSITION;
	float2 tex : TEXCOORD0;
	float fogFactor : FOG;
};

PixelInputType FogVertexShader(VertexInputType input) {
	PixelInputType output;
	float4 cameraPosition;

	input.position.w = 1.0f;

	output.position = mul(input.position, World);
	output.position = mul(output.position, View);
	output.position = mul(output.position, Projection);

	output.tex = input.tex;

	cameraPosition = mul(input.position, World);
	cameraPosition = mul(cameraPosition, View);

	output.fogFactor = saturate((FogEnd - cameraPosition.z) / (FogEnd - FogStart));

	return output;
}