cbuffer MatrixBuffer {
	matrix World;
	matrix View;
	matrix Projection;
};

cbuffer ReflectionBuffer {
	matrix Reflection;
};

struct VertexInputType {
	float4 position : POSITION;
	float2 tex : TEXCOORD0;
};

struct PixelInputType {
	float4 position : SV_POSITION;
	float2 tex : TEXCOORD0;
	float4 reflectionPosition : TEXCOORD1;
};

PixelInputType ReflectionVertexShader(VertexInputType input) {
	PixelInputType output;
	matrix reflectProjectWorld;

	input.position.w = 1.0f;

	output.position = mul(input.position, World);
	output.position = mul(output.position, View);
	output.position = mul(output.position, Projection);
	output.tex = input.tex;

	reflectProjectWorld = mul(Reflection, Projection);
	reflectProjectWorld = mul(World, reflectProjectWorld);

	output.reflectionPosition = mul(input.position, reflectProjectWorld);

	return output;
}