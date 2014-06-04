using Disseminate.Core;
using Disseminate.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disseminate.Graphics
{
    #region BlendStates
    public struct BlendState
    {
        public BlendFunction AlphaBlendFunction;
        public BlendFunction ColorBlendFunction;
        public Blend AlphaDestinationBlend;
        public Blend AlphaSourceBlend;
        public Blend ColorDestinationBlend;
        public Blend ColorSourceBlend;
        public ColorWriteChannels ColorWriteChannels;
        public Color BlendFactor;
        public int MultiSampleMask;

        internal static BlendState _default = new BlendState()
        {
            AlphaBlendFunction = BlendFunction.Add,
            AlphaDestinationBlend = Blend.Zero,
            AlphaSourceBlend = Blend.One,
            ColorBlendFunction = BlendFunction.Add,
            ColorDestinationBlend = Blend.Zero,
            ColorSourceBlend = Blend.One,
            ColorWriteChannels = ColorWriteChannels.All
        };

        private static BlendState _additive = new BlendState()
        {
            AlphaBlendFunction = BlendFunction.Add,
            ColorBlendFunction = BlendFunction.Add,
            ColorWriteChannels = ColorWriteChannels.All,
            ColorSourceBlend = Blend.SourceAlpha,
            AlphaSourceBlend = Blend.SourceAlpha,
            ColorDestinationBlend = Blend.One,
            AlphaDestinationBlend = Blend.One
        };

        public static BlendState additive { get { return _additive; } }

        private static BlendState _alphaBlend = new BlendState()
        {
            AlphaBlendFunction = BlendFunction.Add,
            ColorBlendFunction = BlendFunction.Add,
            ColorWriteChannels = ColorWriteChannels.All,
            ColorSourceBlend = Blend.One,
            AlphaSourceBlend = Blend.One,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            AlphaDestinationBlend = Blend.InverseSourceAlpha
        };

        public static BlendState alphaBlend { get { return _alphaBlend; } }

        private static BlendState _nonPremultiplied = new BlendState()
        {
            AlphaBlendFunction = BlendFunction.Add,
            ColorBlendFunction = BlendFunction.Add,
            ColorWriteChannels = ColorWriteChannels.All,
            ColorSourceBlend = Blend.SourceAlpha,
            AlphaSourceBlend = Blend.SourceAlpha,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            AlphaDestinationBlend = Blend.InverseSourceAlpha
        };

        public static BlendState nonPremultiplied { get { return _nonPremultiplied; } }

        private static BlendState _opaque = new BlendState()
        {
            ColorBlendFunction = BlendFunction.Add,
            ColorWriteChannels = ColorWriteChannels.All,
            ColorSourceBlend = Blend.One,
            AlphaSourceBlend = Blend.One,
            ColorDestinationBlend = Blend.Zero,
            AlphaDestinationBlend = Blend.Zero
        };

        public static BlendState opaque { get { return _opaque; } }
    }

    #endregion

    #region SamplerState
    public struct SamplerState
    {
        public TextureAddressMode AddressU;
        public TextureAddressMode AddressV;
        public TextureAddressMode AddressW;
        public TextureFilter Filter;

        public int MaxAnisotropy;
        public int MaxMipLevel;
        public float MipMapLevelOfDetailBias;

        internal static SamplerState _default = new SamplerState()
        {
            MaxAnisotropy = 4,
            MaxMipLevel = 0,
            MipMapLevelOfDetailBias = 0,
            Filter = TextureFilter.Linear,
            AddressU = TextureAddressMode.Wrap,
            AddressV = TextureAddressMode.Wrap,
            AddressW = TextureAddressMode.Wrap
        };


        private static SamplerState _anisotropicClamp = new SamplerState()
        {
            MaxAnisotropy = 4,
            MaxMipLevel = 0,
            MipMapLevelOfDetailBias = 0,
            Filter = TextureFilter.Anisotropic,
            AddressU = TextureAddressMode.Clamp,
            AddressV = TextureAddressMode.Clamp,
            AddressW = TextureAddressMode.Clamp
        };

        public static SamplerState anisotropicClamp { get { return _anisotropicClamp; } }

        internal static SamplerState _anisotropicWrap = new SamplerState()
        {
            MaxAnisotropy = 4,
            MaxMipLevel = 0,
            MipMapLevelOfDetailBias = 0,
            Filter = TextureFilter.Anisotropic,
            AddressU = TextureAddressMode.Wrap,
            AddressV = TextureAddressMode.Wrap,
            AddressW = TextureAddressMode.Wrap
        };

        public static SamplerState anisotropicWrap { get { return _anisotropicWrap; } }

        internal static SamplerState _linearClamp = new SamplerState()
        {
            MaxAnisotropy = 4,
            MaxMipLevel = 0,
            MipMapLevelOfDetailBias = 0,
            Filter = TextureFilter.Linear,
            AddressU = TextureAddressMode.Clamp,
            AddressV = TextureAddressMode.Clamp,
            AddressW = TextureAddressMode.Clamp
        };

        public static SamplerState linearClamp { get { return _linearClamp; } }

        internal static SamplerState _linearWrap = new SamplerState()
        {
            MaxAnisotropy = 4,
            MaxMipLevel = 0,
            MipMapLevelOfDetailBias = 0,
            Filter = TextureFilter.Linear,
            AddressU = TextureAddressMode.Wrap,
            AddressV = TextureAddressMode.Wrap,
            AddressW = TextureAddressMode.Wrap
        };

        public static SamplerState linearWrap { get { return _linearWrap; } }

        internal static SamplerState _pointClamp = new SamplerState()
        {
            MaxAnisotropy = 4,
            MaxMipLevel = 0,
            MipMapLevelOfDetailBias = 0,
            Filter = TextureFilter.Point,
            AddressU = TextureAddressMode.Clamp,
            AddressV = TextureAddressMode.Clamp,
            AddressW = TextureAddressMode.Clamp
        };

        public static SamplerState pointClamp { get { return _pointClamp; } }

        internal static SamplerState _pointWrap = new SamplerState()
        {
            MaxAnisotropy = 4,
            MaxMipLevel = 0,
            MipMapLevelOfDetailBias = 0,
            Filter = TextureFilter.Point,
            AddressU = TextureAddressMode.Wrap,
            AddressV = TextureAddressMode.Wrap,
            AddressW = TextureAddressMode.Wrap
        };

        public static SamplerState pointWrap { get { return _pointWrap; } }
    }

    #endregion

    #region RasterizerState
    public struct RasterizerState
    {
        public CullMode CullMode;
        public float DepthBias;
        public FillMode FillMode;
        public bool MultiSampleAntiAlias;
        public bool ScissorTestEnable;
        public float SlopeScaleDepthBias;

        internal static RasterizerState _default = new RasterizerState()
        {
            CullMode = CullMode.CullCounterClockwiseFace,
            FillMode = FillMode.Solid,
            DepthBias = 0,
            MultiSampleAntiAlias = true,
            ScissorTestEnable = false,
            SlopeScaleDepthBias = 0
        };

        private static RasterizerState _cullClockwise = new RasterizerState()
        {
            CullMode = CullMode.CullClockwiseFace,
            FillMode = FillMode.Solid,
            DepthBias = 0,
            MultiSampleAntiAlias = true,
            ScissorTestEnable = false,
            SlopeScaleDepthBias = 0,
        };

        public static RasterizerState clockwise { get { return _cullClockwise; } }

        private static RasterizerState _cullCounterClockwise = new RasterizerState()
        {
            CullMode = CullMode.CullCounterClockwiseFace,
            FillMode = FillMode.Solid,
            DepthBias = 0,
            MultiSampleAntiAlias = true,
            ScissorTestEnable = false,
            SlopeScaleDepthBias = 0,
        };

        public static RasterizerState counterClockwise { get { return _cullCounterClockwise; } }

        private static RasterizerState _cullNone = new RasterizerState()
        {
            CullMode = CullMode.None,
            FillMode = FillMode.Solid,
            DepthBias = 0,
            MultiSampleAntiAlias = true,
            ScissorTestEnable = false,
            SlopeScaleDepthBias = 0,
        };

        public static RasterizerState none { get { return _cullNone; } }
    }
    #endregion

    #region Stencil
    public struct DepthStencilState
    {
        public StencilOperation CounterClockwiseStencilDepthBufferFail;
        public StencilOperation CounterClockwiseStencilFail;
        public StencilOperation CounterClockwiseStencilPass;
        public StencilOperation StencilDepthBufferFail;
        public StencilOperation StencilPass;
        public StencilOperation StencilFail;
        public CompareFunction StencilFunction;
        public CompareFunction CounterClockwiseStencilFunction;
        public CompareFunction DepthBufferFunction;
        public int ReferenceStencil;
        public int StencilWriteMask;
        public int StencilMask;
        public bool DepthBufferEnable;
        public bool DepthBufferWriteEnable;
        public bool TwoSidedStencilMode;
        public bool StencilEnable;

        internal static DepthStencilState _default = new DepthStencilState()
        {
            DepthBufferEnable = true,
            DepthBufferWriteEnable = true,
            DepthBufferFunction = CompareFunction.LessEqual,
            StencilEnable = false,
            StencilFunction = CompareFunction.Always,
            StencilPass = StencilOperation.Keep,
            StencilFail = StencilOperation.Keep,
            StencilDepthBufferFail = StencilOperation.Keep,
            TwoSidedStencilMode = false,
            CounterClockwiseStencilFunction = CompareFunction.Always,
            CounterClockwiseStencilFail = StencilOperation.Keep,
            CounterClockwiseStencilPass = StencilOperation.Keep,
            CounterClockwiseStencilDepthBufferFail = StencilOperation.Keep,
            StencilMask = Int32.MaxValue,
            StencilWriteMask = Int32.MaxValue,
            ReferenceStencil = 0
        };

        public static DepthStencilState normal { get { return _default; } }

        internal static DepthStencilState _depthRead = new DepthStencilState()
        {
            DepthBufferEnable = true,
            DepthBufferWriteEnable = false,
            DepthBufferFunction = CompareFunction.LessEqual,
            StencilEnable = false,
            StencilFunction = CompareFunction.Always,
            StencilPass = StencilOperation.Keep,
            StencilFail = StencilOperation.Keep,
            StencilDepthBufferFail = StencilOperation.Keep,
            TwoSidedStencilMode = false,
            CounterClockwiseStencilFunction = CompareFunction.Always,
            CounterClockwiseStencilFail = StencilOperation.Keep,
            CounterClockwiseStencilPass = StencilOperation.Keep,
            CounterClockwiseStencilDepthBufferFail = StencilOperation.Keep,
            StencilMask = Int32.MaxValue,
            StencilWriteMask = Int32.MaxValue,
            ReferenceStencil = 0
        };

        public static DepthStencilState read { get { return _depthRead; } }

        internal static DepthStencilState _none = new DepthStencilState()
        {
            DepthBufferEnable = false,
            DepthBufferWriteEnable = false,
            DepthBufferFunction = CompareFunction.LessEqual,
            StencilEnable = false,
            StencilFunction = CompareFunction.Always,
            StencilPass = StencilOperation.Keep,
            StencilFail = StencilOperation.Keep,
            StencilDepthBufferFail = StencilOperation.Keep,
            TwoSidedStencilMode = false,
            CounterClockwiseStencilFunction = CompareFunction.Always,
            CounterClockwiseStencilFail = StencilOperation.Keep,
            CounterClockwiseStencilPass = StencilOperation.Keep,
            CounterClockwiseStencilDepthBufferFail = StencilOperation.Keep,
            StencilMask = Int32.MaxValue,
            StencilWriteMask = Int32.MaxValue,
            ReferenceStencil = 0
        };

        public static DepthStencilState none { get { return _none; } }

    }
    #endregion
}
