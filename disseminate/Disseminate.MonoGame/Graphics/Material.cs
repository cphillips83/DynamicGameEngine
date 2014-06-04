using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disseminate.Graphics;
using Disseminate.Math;
using XnaBlendState = Microsoft.Xna.Framework.Graphics.BlendState;
using XnaBlendFunc = Microsoft.Xna.Framework.Graphics.BlendFunction;
using XnaBlend = Microsoft.Xna.Framework.Graphics.Blend;
using XnaBlendChannel = Microsoft.Xna.Framework.Graphics.ColorWriteChannels;

using XnaSamplerState = Microsoft.Xna.Framework.Graphics.SamplerState;
using XnaTextureAddressMode = Microsoft.Xna.Framework.Graphics.TextureAddressMode;
using XnaTextureFilter = Microsoft.Xna.Framework.Graphics.TextureFilter;

using XnaRasterizerState = Microsoft.Xna.Framework.Graphics.RasterizerState;
using XnaCullMode = Microsoft.Xna.Framework.Graphics.CullMode;
using XnaFillMode = Microsoft.Xna.Framework.Graphics.FillMode;

using XnaDepthState = Microsoft.Xna.Framework.Graphics.DepthStencilState;
using XnaStencilOperation = Microsoft.Xna.Framework.Graphics.StencilOperation;
using XnaCompareFunction = Microsoft.Xna.Framework.Graphics.CompareFunction;

using Disseminate.Core;
 
namespace Disseminate.MonoGame.Graphics
{
    public class Material : GLMaterial
    {
        public Material(ushort id, string name)
            : base(id, name)
        {
        }

        public bool isTransparent { get; private set; }

        public override void enable()
        {
            var mgl = MonoGL.instance;
            mgl.blendState = BlendState;
            mgl.depthState = DepthStencilState;
            mgl.rasterizerState = RasterizerState;
            mgl.scissorRasterizerState = ScissorRasterizerState;
            mgl.samplerState = SamplerState;
            mgl.materialState = this;
        }

        #region XnaBlendState
        private XnaBlendState blendState = new XnaBlendState();

        internal XnaBlendState BlendState
        {
            get
            {
                if (blendStateDirty)
                {
                    UpdateBlendState(blendState);
                }

                return blendState;
            }
        }

        public void SetBlendState(XnaBlendState state)
        {
            AlphaBlendFunction = (BlendFunction)state.AlphaBlendFunction;
            AlphaDestinationBlend = (Blend)state.AlphaDestinationBlend;
            AlphaSourceBlend = (Blend)state.AlphaSourceBlend;
            BlendFactor = state.BlendFactor.ToColor();
            ColorBlendFunction = (BlendFunction)state.ColorBlendFunction;
            ColorDestinationBlend = (Blend)state.ColorDestinationBlend;
            ColorSourceBlend = (Blend)state.ColorSourceBlend;
            ColorWriteChannels = (ColorWriteChannels)state.ColorWriteChannels;
            ColorWriteChannels1 = (ColorWriteChannels)state.ColorWriteChannels1;
            ColorWriteChannels2 = (ColorWriteChannels)state.ColorWriteChannels2;
            ColorWriteChannels3 = (ColorWriteChannels)state.ColorWriteChannels3;
            MultiSampleMask = state.MultiSampleMask;
            blendStateDirty = true;
        }

        internal void UpdateBlendState(XnaBlendState state)
        {
            state.AlphaBlendFunction = (XnaBlendFunc)AlphaBlendFunction;
            state.AlphaDestinationBlend = (XnaBlend)AlphaDestinationBlend;
            state.AlphaSourceBlend = (XnaBlend)AlphaSourceBlend;
            state.BlendFactor = BlendFactor.ToXnaColor();
            state.ColorBlendFunction = (XnaBlendFunc)ColorBlendFunction;
            state.ColorDestinationBlend = (XnaBlend)ColorDestinationBlend;
            state.ColorSourceBlend = (XnaBlend)ColorSourceBlend;
            state.ColorWriteChannels = (XnaBlendChannel)ColorWriteChannels;
            state.ColorWriteChannels1 = (XnaBlendChannel)ColorWriteChannels1;
            state.ColorWriteChannels2 = (XnaBlendChannel)ColorWriteChannels2;
            state.ColorWriteChannels3 = (XnaBlendChannel)ColorWriteChannels3;
            state.MultiSampleMask = MultiSampleMask;
            blendStateDirty = false;
        }
        #endregion

        #region XnaSamplerState
        private XnaSamplerState samplerState = new XnaSamplerState();

        internal XnaSamplerState SamplerState
        {
            get
            {
                if (samplerStateDirty)
                {
                    UpdateSamplerState(samplerState);
                }

                return samplerState;
            }
        }

        public void SetSamplerState(XnaSamplerState state)
        {
            AddressU = (TextureAddressMode)state.AddressU;
            AddressV = (TextureAddressMode)state.AddressV;
            AddressW = (TextureAddressMode)state.AddressW;
            Filter = (TextureFilter)state.Filter;
            MaxAnisotropy = state.MaxAnisotropy;
            MaxMipLevel = state.MaxMipLevel;
            MipMapLevelOfDetailBias = state.MipMapLevelOfDetailBias;
        }

        internal void UpdateSamplerState(XnaSamplerState state)
        {
            state.AddressU = (XnaTextureAddressMode)AddressU;
            state.AddressV = (XnaTextureAddressMode)AddressV;
            state.AddressW = (XnaTextureAddressMode)AddressW;
            state.Filter = (XnaTextureFilter)Filter;
            state.MaxAnisotropy = MaxAnisotropy;
            state.MaxMipLevel = MaxMipLevel;
            state.MipMapLevelOfDetailBias = MipMapLevelOfDetailBias;
            samplerStateDirty = false;
        }
        #endregion

        #region XnaRasterizerState
        private XnaRasterizerState rasterizerState = new XnaRasterizerState();
        private XnaRasterizerState scissorRasterizerState = new XnaRasterizerState();

        internal XnaRasterizerState RasterizerState
        {
            get
            {
                if (rasterizerStateDirty)
                {
                    UpdateRasterizerState(rasterizerState);
                    UpdateRasterizerState(scissorRasterizerState);
                    scissorRasterizerState.ScissorTestEnable = true;
                }

                return rasterizerState;
            }
        }

        internal XnaRasterizerState ScissorRasterizerState
        {
            get
            {
                if (rasterizerStateDirty)
                {
                    UpdateRasterizerState(rasterizerState);
                    UpdateRasterizerState(scissorRasterizerState);
                    scissorRasterizerState.ScissorTestEnable = true;
                }

                return scissorRasterizerState;
            }
        }

        public void SetRasterizerState(XnaRasterizerState state)
        {
            cullMode = (CullMode)state.CullMode;
            depthBias = state.DepthBias;
            fillMode = (FillMode)state.FillMode;
            multiSampleAntiAlias = state.MultiSampleAntiAlias;
            slopeScaleDepthBias = state.SlopeScaleDepthBias;
        }

        internal void UpdateRasterizerState(XnaRasterizerState state)
        {
            state.CullMode = (XnaCullMode)cullMode;
            state.DepthBias = depthBias;
            state.FillMode = (XnaFillMode)fillMode;
            state.MultiSampleAntiAlias = multiSampleAntiAlias;
            state.SlopeScaleDepthBias = slopeScaleDepthBias;
            rasterizerStateDirty = false;
        }
        #endregion

        #region XnaDepthStencilState
        private XnaDepthState depthStencilState = new XnaDepthState();

        internal XnaDepthState DepthStencilState
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

        public void SetDepthStencilState(XnaDepthState state)
        {
            CounterClockwiseStencilDepthBufferFail = (StencilOperation)state.CounterClockwiseStencilDepthBufferFail;
            CounterClockwiseStencilFail = (StencilOperation)state.CounterClockwiseStencilFail;
            CounterClockwiseStencilFunction = (CompareFunction)state.CounterClockwiseStencilFunction;
            CounterClockwiseStencilPass = (StencilOperation)state.CounterClockwiseStencilPass;
            DepthBufferEnable = state.DepthBufferEnable;
            DepthBufferFunction = (CompareFunction)state.DepthBufferFunction;
            DepthBufferWriteEnable = state.DepthBufferWriteEnable;
            ReferenceStencil = state.ReferenceStencil;
            StencilDepthBufferFail = (StencilOperation)state.StencilDepthBufferFail;
            StencilEnable = state.StencilEnable;
            StencilFail = (StencilOperation)state.StencilFail;
            StencilFunction = (CompareFunction)state.StencilFunction;
            StencilMask = state.StencilMask;
            StencilPass = (StencilOperation)state.StencilPass;
            StencilWriteMask = state.StencilWriteMask;
            TwoSidedStencilMode = state.TwoSidedStencilMode;

        }

        public void UpdateDepthStencilState(XnaDepthState state)
        {
            state.CounterClockwiseStencilDepthBufferFail = (XnaStencilOperation)CounterClockwiseStencilDepthBufferFail;
            state.CounterClockwiseStencilFail = (XnaStencilOperation)CounterClockwiseStencilFail;
            state.CounterClockwiseStencilFunction = (XnaCompareFunction)CounterClockwiseStencilFunction;
            state.CounterClockwiseStencilPass = (XnaStencilOperation)CounterClockwiseStencilPass;
            state.DepthBufferEnable = DepthBufferEnable;
            state.DepthBufferFunction = (XnaCompareFunction)DepthBufferFunction;
            state.DepthBufferWriteEnable = DepthBufferWriteEnable;
            state.ReferenceStencil = ReferenceStencil;
            state.StencilDepthBufferFail = (XnaStencilOperation)StencilDepthBufferFail;
            state.StencilEnable = StencilEnable;
            state.StencilFail = (XnaStencilOperation)StencilFail;
            state.StencilFunction = (XnaCompareFunction)StencilFunction;
            state.StencilMask = StencilMask;
            state.StencilPass = (XnaStencilOperation)StencilPass;
            state.StencilWriteMask = StencilWriteMask;
            state.TwoSidedStencilMode = TwoSidedStencilMode;
            depthStencilStateDirty = false;
        }
        #endregion
    }

}
