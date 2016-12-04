cbuffer PerFrameBuffer {
	matrix World;
	matrix View;
	matrix Projection;
};

struct VertexInputType {
	float4 position : POSITION;
	float2 tex : TEXCOORD0;
};

struct PixelInputType {
	float4 position : SV_POSITION;
	float2 tex : TEXCOORD0;
};

PixelInputType FontVertexShader(VertexInputType input) {
	PixelInputType output;

	input.position.w = 1.0f;

	output.position = mul(input.position, World);
	output.position = mul(output.position, View);
	output.position = mul(output.position, Projection);
	output.tex = input.tex;

	return output;
}