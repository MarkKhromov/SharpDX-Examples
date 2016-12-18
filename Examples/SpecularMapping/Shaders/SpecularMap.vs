cbuffer MatrixBuffer {
	matrix World;
	matrix View;
	matrix Projection;
};

cbuffer CameraBuffer {
	float3 CameraPosition;
};

struct VertexInputType {
	float4 position : POSITION;
	float2 tex : TEXCOORD0;
	float3 normal : NORMAL;
	float3 tangent : TANGENT;
	float3 binormal : BINORMAL;
};

struct PixelInputType {
	float4 position : SV_POSITION;
	float2 tex : TEXCOORD0;
	float3 normal : NORMAL;
	float3 tangent : TANGENT;
	float3 binormal : BINORMAL;
	float3 viewDirection : TEXCOORD1;
};

PixelInputType SpecularMapVertexShader(VertexInputType input) {
	PixelInputType output;
	float4 worldPosition;

	input.position.w = 1.0f;

	output.position = mul(input.position, World);
	output.position = mul(output.position, View);
	output.position = mul(output.position, Projection);
	output.tex = input.tex;
	output.normal = mul(input.normal, (float3x3)World);
	output.normal = normalize(output.normal);
	output.tangent = mul(input.tangent, (float3x3)World);
	output.tangent = normalize(output.tangent);
	output.binormal = mul(input.binormal, (float3x3)World);
	output.binormal = normalize(output.binormal);
	worldPosition = mul(input.position, World);
	output.viewDirection = CameraPosition.xyz - worldPosition.xyz;
	output.viewDirection = normalize(output.viewDirection);

	return output;
}