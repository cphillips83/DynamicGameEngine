using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Disseminate.MonoGame.Graphics
{
    public class GraphicsState
    {
        public Effect effect = null;

        #region BlendState
        private bool blendStateDirty = true;
        private BlendState blendState = new BlendState();

        internal BlendState BlendState
        {
            get
            {
                if (blendStateDirty)
                {
                    UpdateBlendState(blendState);
                    blendStateDirty = false;
                }

                return blendState;
            }
        }

        private BlendFunction _AlphaBlendFunction;
        private Blend _AlphaDestinationBlend;
        private Blend _AlphaSourceBlend;
        private Color _BlendFactor;
        private BlendFunction _ColorBlendFunction;
        private Blend _ColorDestinationBlend;
        private Blend _ColorSourceBlend;
        private ColorWriteChannels _ColorWriteChannels;
        private ColorWriteChannels _ColorWriteChannels1;
        private ColorWriteChannels _ColorWriteChannels2;
        private ColorWriteChannels _ColorWriteChannels3;
        private int _MultiSampleMask;

        public BlendFunction AlphaBlendFunction { get { return _AlphaBlendFunction; } set { _AlphaBlendFunction = value; blendStateDirty = true; } }
        public Blend AlphaDestinationBlend { get { return _AlphaDestinationBlend; } set { _AlphaDestinationBlend = value; blendStateDirty = true; } }
        public Blend AlphaSourceBlend { get { return _AlphaSourceBlend; } set { _AlphaSourceBlend = value; blendStateDirty = true; } }
        public Color BlendFactor { get { return _BlendFactor; } set { _BlendFactor = value; blendStateDirty = true; } }
        public BlendFunction ColorBlendFunction { get { return _ColorBlendFunction; } set { _ColorBlendFunction = value; blendStateDirty = true; } }
        public Blend ColorDestinationBlend { get { return _ColorDestinationBlend; } set { _ColorDestinationBlend = value; blendStateDirty = true; } }
        public Blend ColorSourceBlend { get { return _ColorSourceBlend; } set { _ColorSourceBlend = value; blendStateDirty = true; } }
        public ColorWriteChannels ColorWriteChannels { get { return _ColorWriteChannels; } set { _ColorWriteChannels = value; blendStateDirty = true; } }
        public ColorWriteChannels ColorWriteChannels1 { get { return _ColorWriteChannels1; } set { _ColorWriteChannels1 = value; blendStateDirty = true; } }
        public ColorWriteChannels ColorWriteChannels2 { get { return _ColorWriteChannels2; } set { _ColorWriteChannels2 = value; blendStateDirty = true; } }
        public ColorWriteChannels ColorWriteChannels3 { get { return _ColorWriteChannels3; } set { _ColorWriteChannels3 = value; blendStateDirty = true; } }
        public int MultiSampleMask { get { return _MultiSampleMask; } set { _MultiSampleMask = value; blendStateDirty = true; } }

        public void SetBlendState(BlendState state)
        {
            _AlphaBlendFunction = state.AlphaBlendFunction;
            _AlphaDestinationBlend = state.AlphaDestinationBlend;
            _AlphaSourceBlend = state.AlphaSourceBlend;
            _BlendFactor = state.BlendFactor;
            _ColorBlendFunction = state.ColorBlendFunction;
            _ColorDestinationBlend = state.ColorDestinationBlend;
            _ColorSourceBlend = state.ColorSourceBlend;
            _ColorWriteChannels = state.ColorWriteChannels;
            _ColorWriteChannels1 = state.ColorWriteChannels1;
            _ColorWriteChannels2 = state.ColorWriteChannels2;
            _ColorWriteChannels3 = state.ColorWriteChannels3;
            _MultiSampleMask = state.MultiSampleMask;
            blendStateDirty = true;
        }

        internal void UpdateBlendState(BlendState state)
        {
            state.AlphaBlendFunction = _AlphaBlendFunction;
            state.AlphaDestinationBlend = _AlphaDestinationBlend;
            state.AlphaSourceBlend = _AlphaSourceBlend;
            state.BlendFactor = _BlendFactor;
            state.ColorBlendFunction = _ColorBlendFunction;
            state.ColorDestinationBlend = _ColorDestinationBlend;
            state.ColorSourceBlend = _ColorSourceBlend;
            state.ColorWriteChannels = _ColorWriteChannels;
            state.ColorWriteChannels1 = _ColorWriteChannels1;
            state.ColorWriteChannels2 = _ColorWriteChannels2;
            state.ColorWriteChannels3 = _ColorWriteChannels3;
            state.MultiSampleMask = _MultiSampleMask;
        }
        #endregion

        #region SamplerState
        private bool samplerStateDirty = true;
        private SamplerState samplerState = new SamplerState();

        internal SamplerState SamplerState
        {
            get
            {
                if (samplerStateDirty)
                {
                    UpdateSamplerState(samplerState);
                    samplerStateDirty = false;
                }

                return samplerState;
            }
        }

        private TextureAddressMode _AddressU = TextureAddressMode.Clamp;
        private TextureAddressMode _AddressV = TextureAddressMode.Clamp;
        private TextureAddressMode _AddressW = TextureAddressMode.Clamp;
        private TextureFilter _Filter = TextureFilter.Point;
        private int _MaxAnisotropy = 0;
        private int _MaxMipLevel = 0;
        private float _MipMapLevelOfDetailBias = 0;

        public TextureAddressMode AddressU { get { return _AddressU; } set { _AddressU = value; samplerStateDirty = true; } }
        public TextureAddressMode AddressV { get { return _AddressV; } set { _AddressV = value; samplerStateDirty = true; } }
        public TextureAddressMode AddressW { get { return _AddressW; } set { _AddressW = value; samplerStateDirty = true; } }
        public TextureFilter Filter { get { return _Filter; } set { _Filter = value; samplerStateDirty = true; } }
        public int MaxAnisotropy { get { return _MaxAnisotropy; } set { _MaxAnisotropy = value; samplerStateDirty = true; } }
        public int MaxMipLevel { get { return _MaxMipLevel; } set { _MaxMipLevel = value; samplerStateDirty = true; } }
        public float MipMapLevelOfDetailBias { get { return _MipMapLevelOfDetailBias; } set { _MipMapLevelOfDetailBias = value; samplerStateDirty = true; } }

        public void SetSamplerState(SamplerState state)
        {
            _AddressU = state.AddressU;
            _AddressV = state.AddressV;
            _AddressW = state.AddressW;
            _Filter = state.Filter;
            _MaxAnisotropy = state.MaxAnisotropy;
            _MaxMipLevel = state.MaxMipLevel;
            _MipMapLevelOfDetailBias = state.MipMapLevelOfDetailBias;
            samplerStateDirty = true;
        }

        internal void UpdateSamplerState(SamplerState state)
        {
            state.AddressU = _AddressU;
            state.AddressV = _AddressV;
            state.AddressW = _AddressW;
            state.Filter = _Filter;
            state.MaxAnisotropy = _MaxAnisotropy;
            state.MaxMipLevel = _MaxMipLevel;
            state.MipMapLevelOfDetailBias = _MipMapLevelOfDetailBias;
        }
        #endregion

        #region RastizerState
        private bool rasterizerStateDirty = true;
        private RasterizerState rasterizerState = new RasterizerState();
        private RasterizerState scissorRasterizerState = new RasterizerState();

        internal RasterizerState RasterizerState
        {
            get
            {
                if (rasterizerStateDirty)
                {
                    UpdateRasterizerState(rasterizerState);
                    UpdateRasterizerState(scissorRasterizerState);
                    scissorRasterizerState.ScissorTestEnable = true;
                    rasterizerStateDirty = false;
                }

                return rasterizerState;
            }
        }

        internal RasterizerState ScissorRasterizerState
        {
            get
            {
                if (rasterizerStateDirty)
                {
                    UpdateRasterizerState(rasterizerState);
                    UpdateRasterizerState(scissorRasterizerState);
                    scissorRasterizerState.ScissorTestEnable = true;
                    rasterizerStateDirty = false;
                }

                return scissorRasterizerState;
            }
        }

        private CullMode _cullMode = CullMode.CullCounterClockwiseFace;
        private float _depthBias = 0f;
        private FillMode _fillMode = FillMode.Solid;
        private bool _multiSampleAntiAlias = true;
        private float _slopeScaleDepthBias = 0f;

        public CullMode cullMode { get { return _cullMode; } set { _cullMode = value; rasterizerStateDirty = true; } }
        public float depthBias { get { return _depthBias; } set { _depthBias = value; rasterizerStateDirty = true; } }
        public FillMode fillMode { get { return _fillMode; } set { _fillMode = value; rasterizerStateDirty = true; } }
        public bool multiSampleAntiAlias { get { return _multiSampleAntiAlias; } set { _multiSampleAntiAlias = value; rasterizerStateDirty = true; } }
        public float slopeScaleDepthBias { get { return _slopeScaleDepthBias; } set { _slopeScaleDepthBias = value; rasterizerStateDirty = true; } }

        public void SetRasterizerState(RasterizerState state)
        {
            _cullMode = state.CullMode;
            _depthBias = state.DepthBias;
            _fillMode = state.FillMode;
            _multiSampleAntiAlias = state.MultiSampleAntiAlias;
            _slopeScaleDepthBias = state.SlopeScaleDepthBias;
            rasterizerStateDirty = true;
        }

        internal void UpdateRasterizerState(RasterizerState state)
        {
            state.CullMode = _cullMode;
            state.DepthBias = _depthBias;
            state.FillMode = _fillMode;
            state.MultiSampleAntiAlias = _multiSampleAntiAlias;
            state.SlopeScaleDepthBias = _slopeScaleDepthBias;
        }

        #endregion RastizerState

        #region DepthStencilState
        private bool depthStencilStateDirty = true;
        private DepthStencilState depthStencilState = new DepthStencilState();

        internal DepthStencilState DepthStencilState
        {
            get
            {
                if (depthStencilStateDirty)
                {
                    UpdateDepthStencilState(depthStencilState);
                    depthStencilStateDirty = false;
                }

                return depthStencilState;
            }
        }

        private StencilOperation _CounterClockwiseStencilDepthBufferFail;
        private StencilOperation _CounterClockwiseStencilFail;
        private CompareFunction _CounterClockwiseStencilFunction;
        private StencilOperation _CounterClockwiseStencilPass;
        private CompareFunction _DepthBufferFunction;
        private StencilOperation _StencilDepthBufferFail;
        private StencilOperation _StencilPass;
        private StencilOperation _StencilFail;
        private CompareFunction _StencilFunction;
        private bool _DepthBufferEnable;
        private bool _DepthBufferWriteEnable;
        private bool _TwoSidedStencilMode;
        private bool _StencilEnable;
        private int _ReferenceStencil;
        private int _StencilMask;
        private int _StencilWriteMask;

        public StencilOperation CounterClockwiseStencilDepthBufferFail { get { return _CounterClockwiseStencilDepthBufferFail; } set { _CounterClockwiseStencilDepthBufferFail = value; depthStencilStateDirty = true; } }
        public StencilOperation CounterClockwiseStencilFail { get { return _CounterClockwiseStencilFail; } set { _CounterClockwiseStencilFail = value; depthStencilStateDirty = true; } }
        public CompareFunction CounterClockwiseStencilFunction { get { return _CounterClockwiseStencilFunction; } set { _CounterClockwiseStencilFunction = value; depthStencilStateDirty = true; } }
        public StencilOperation CounterClockwiseStencilPass { get { return _CounterClockwiseStencilPass; } set { _CounterClockwiseStencilPass = value; depthStencilStateDirty = true; } }
        public CompareFunction DepthBufferFunction { get { return _DepthBufferFunction; } set { _DepthBufferFunction = value; depthStencilStateDirty = true; } }
        public StencilOperation StencilDepthBufferFail { get { return _StencilDepthBufferFail; } set { _StencilDepthBufferFail = value; depthStencilStateDirty = true; } }
        public StencilOperation StencilPass { get { return _StencilPass; } set { _StencilPass = value; depthStencilStateDirty = true; } }
        public StencilOperation StencilFail { get { return _StencilFail; } set { _StencilFail = value; depthStencilStateDirty = true; } }
        public CompareFunction StencilFunction { get { return _StencilFunction; } set { _StencilFunction = value; depthStencilStateDirty = true; } }
        public bool DepthBufferEnable { get { return _DepthBufferEnable; } set { _DepthBufferEnable = value; depthStencilStateDirty = true; } }
        public bool DepthBufferWriteEnable { get { return _DepthBufferWriteEnable; } set { _DepthBufferWriteEnable = value; depthStencilStateDirty = true; } }
        public bool TwoSidedStencilMode { get { return _TwoSidedStencilMode; } set { _TwoSidedStencilMode = value; depthStencilStateDirty = true; } }
        public bool StencilEnable { get { return _StencilEnable; } set { _StencilEnable = value; depthStencilStateDirty = true; } }
        public int ReferenceStencil { get { return _ReferenceStencil; } set { _ReferenceStencil = value; depthStencilStateDirty = true; } }
        public int StencilMask { get { return _StencilMask; } set { _StencilMask = value; depthStencilStateDirty = true; } }
        public int StencilWriteMask { get { return _StencilWriteMask; } set { _StencilWriteMask = value; depthStencilStateDirty = true; } }

        public void SetDepthStencilState(DepthStencilState state)
        {
            _CounterClockwiseStencilDepthBufferFail = state.CounterClockwiseStencilDepthBufferFail;
            _CounterClockwiseStencilFail = state.CounterClockwiseStencilFail;
            _CounterClockwiseStencilFunction = state.CounterClockwiseStencilFunction;
            _CounterClockwiseStencilPass = state.CounterClockwiseStencilPass;
            _DepthBufferEnable = state.DepthBufferEnable;
            _DepthBufferFunction = state.DepthBufferFunction;
            _DepthBufferWriteEnable = state.DepthBufferWriteEnable;
            _ReferenceStencil = state.ReferenceStencil;
            _StencilDepthBufferFail = state.StencilDepthBufferFail;
            _StencilEnable = state.StencilEnable;
            _StencilFail = state.StencilFail;
            _StencilFunction = state.StencilFunction;
            _StencilMask = state.StencilMask;
            _StencilPass = state.StencilPass;
            _StencilWriteMask = state.StencilWriteMask;
            _TwoSidedStencilMode = state.TwoSidedStencilMode;
            depthStencilStateDirty = true;

        }

        public void UpdateDepthStencilState(DepthStencilState state)
        {
            state.CounterClockwiseStencilDepthBufferFail = _CounterClockwiseStencilDepthBufferFail;
            state.CounterClockwiseStencilFail = _CounterClockwiseStencilFail;
            state.CounterClockwiseStencilFunction = _CounterClockwiseStencilFunction;
            state.CounterClockwiseStencilPass = _CounterClockwiseStencilPass;
            state.DepthBufferEnable = _DepthBufferEnable;
            state.DepthBufferFunction = _DepthBufferFunction;
            state.DepthBufferWriteEnable = _DepthBufferWriteEnable;
            state.ReferenceStencil = _ReferenceStencil;
            state.StencilDepthBufferFail = _StencilDepthBufferFail;
            state.StencilEnable = _StencilEnable;
            state.StencilFail = _StencilFail;
            state.StencilFunction = _StencilFunction;
            state.StencilMask = _StencilMask;
            state.StencilPass = _StencilPass;
            state.StencilWriteMask = _StencilWriteMask;
            state.TwoSidedStencilMode = _TwoSidedStencilMode;

        }

        #endregion

        internal GraphicsState()
        {
            SetSamplerState(SamplerState.PointClamp);
            SetBlendState(BlendState.NonPremultiplied);
            SetRasterizerState(RasterizerState.CullCounterClockwise);
            SetDepthStencilState(DepthStencilState.Default);
        }
    }
}
