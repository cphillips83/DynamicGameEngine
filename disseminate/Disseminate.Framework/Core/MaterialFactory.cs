using Disseminate.Graphics;
using Disseminate.Json;
using Disseminate.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Disseminate.Core
{
    public abstract class AbstractMaterialFactory<T>
        where T : GLMaterial
    {
        protected Dictionary<string, T> _materials = new Dictionary<string, T>();
        protected AbstractResourceFactory _resources;
        private ushort[] _freeIds = new ushort[ushort.MaxValue];
        private int _freeIndex = ushort.MaxValue - 1;

        public AbstractMaterialFactory(AbstractResourceFactory rf)
        {
            _resources = rf;
            for (var i = 0; i < _freeIds.Length; i++)
                _freeIds[ushort.MaxValue - i - 1] = (ushort)i;

        }

        private ushort getNextId()
        {
            if (_freeIndex == 0)
                throw new Exception("No more entities free!");

            _freeIndex--;
            var id = _freeIds[_freeIndex];

            return id;
        }

        private void freeId(ushort id)
        {
            _freeIds[_freeIndex] = id;
            _freeIndex++;
        }

        public T create()
        {
            return create(null);
        }

        public T create(string name)
        {
            var id = getNextId();
            if (string.IsNullOrEmpty(name))
                name = string.Format("_material_{0}_", id);

            name = name.ToLower();

            var t = createMaterial(id, name);
            _materials.Add(name, t);

            return t;
        }

        public T get(string name)
        {
            name = name.ToLower();
            return _materials[name];
        }

        //public T find(string name)
        //{
        //    name = name.ToLower();
        //    T t;

        //    if (_materials.TryGetValue(name, out t))
        //        return t;

        //    return load(name);
        //}

        public T load(string name, string file)
        {
            //name = name.ToLower();

            //remove .json
            //var matname = name.Replace(".json", string.Empty);//System.IO.Path.GetFileNameWithoutExtension(name);

            var t = default(T);
            if (!_materials.TryGetValue(name, out t))
            {
                var location = _resources.file(file);
                if (location == null)
                {
                    t = create(name);
                }
                else
                {
                    t = process(location);
                    if (t == null)
                    {
                        t = create(name);
                    }
                    else
                    {
                        _materials.Add(name, t);
                    }
                }
            }

            return t;
        }

        public void unload(string name)
        {
            T t;
            if (_materials.TryGetValue(name, out t))
            {
                freeId(t.id);
                _materials.Remove(name);
                t.Dispose();
            }
        }

        public void clear()
        {
            var materials = _materials.Keys.ToArray();
            foreach (var m in materials)
                unload(m);
        }

        protected T process(string file)
        {
            using (var sr = new StreamReader(file))
            {
                var name = System.IO.Path.GetFileNameWithoutExtension(file).ToLower();
                var jo = JsonReader.ParseObject(sr.ReadToEnd());

                return process(name, jo);
            }
        }

        protected T process(string name, JsonObject jo)
        {
            var id = getNextId();
            var m = createMaterial(id, name);
            var blend = BlendState.nonPremultiplied;
            var sampler = SamplerState.pointClamp;
            var rasterizer = RasterizerState.counterClockwise;
            var depth = DepthStencilState.normal;

            if (jo != null)
            {
                processBlendState(ref blend, jo["blend"]);
                processBlendState(ref blend, jo["blend_mode"]);
                processSamplerState(ref sampler, jo["sampler"]);
                processSamplerState(ref sampler, jo["sampler_mode"]);
                processRasterizerState(ref rasterizer, jo["rasterizer"]);
                processRasterizerState(ref rasterizer, jo["rasterizer_mode"]);
                processDepthStencilState(ref depth, jo["depth"]);
                processDepthStencilState(ref depth, jo["depth_mode"]);
                processDepthStencilState(ref depth, jo["stencil"]);
                processDepthStencilState(ref depth, jo["stencil_mode"]);
            }

            m.SetBlendState(blend);
            m.SetSamplerState(sampler);
            m.SetRasterizerState(rasterizer);
            m.SetDepthStencilState(depth);

            return m;
        }

        protected abstract T createMaterial(ushort id, string name);

        #region Processing Logic
        private JsonValue findFirst(JsonObject jo, params string[] args)
        {
            for (var i = 0; i < args.Length; i++)
            {
                var jv = jo[args[i]];
                if (jv != null && jv.Type != JsonTypes.Null)
                    return jv;
            }

            return JsonNull.Null;
        }

        private void processBlendFunction(ref BlendFunction func, JsonObject jo, params string[] args)
        {
            var jv = findFirst(jo, args);
            if (jv.Type == JsonTypes.String)
            {
                var name = (string)jv;
                switch (name.ToLower())
                {
                    case "add": func = BlendFunction.Add; break;
                    case "sub":
                    case "subtract": func = BlendFunction.Subtract; break;
                    case "rsub":
                    case "rsubtract":
                    case "reversesubtract": func = BlendFunction.ReverseSubtract; break;
                    case "max": func = BlendFunction.Max; break;
                    case "min": func = BlendFunction.Min; break;
                }
            }
        }

        private void processBlend(ref Blend blend, JsonObject jo, params string[] args)
        {
            var jv = findFirst(jo, args);
            if (jv.Type == JsonTypes.Number)
            {
                var num = (int)jv;
                switch (num)
                {
                    case 0: blend = Blend.Zero; break;
                    case 1: blend = Blend.One; break;
                }
            }
            else if (jv.Type == JsonTypes.String)
            {
                var name = (string)jv;
                switch (name.ToLower())
                {
                    case "1":
                    case "one": blend = Blend.One; break;
                    case "0":
                    case "zero": blend = Blend.Zero; break;
                    case "srccolor":
                    case "src_color":
                    case "src-color":
                    case "sourcecolor": blend = Blend.SourceColor; break;
                    case "srcalpha":
                    case "src_alpha":
                    case "src-alpha":
                    case "sourcealpha": blend = Blend.SourceAlpha; break;
                    case "saturation":
                    case "alphasaturation":
                    case "alpha_saturation":
                    case "alpha-saturation":
                    case "source_alpha_saturation":
                    case "source-alpha-saturation":
                    case "sourcealphasaturation": blend = Blend.SourceAlphaSaturation; break;
                    case "invsrccolor":
                    case "inv_src_color":
                    case "inv-src-color":
                    case "inversesourcecolor": blend = Blend.InverseSourceColor; break;
                    case "invsrcalpha":
                    case "inv_src_alpha":
                    case "inv-src-alpha":
                    case "inverse_source_alpha":
                    case "inverse-source-alpha":
                    case "inversesourcealpha": blend = Blend.InverseSourceAlpha; break;
                    case "dstcolor":
                    case "dst_color":
                    case "dst-color":
                    case "destination_color":
                    case "destination-color":
                    case "destinationcolor": blend = Blend.DestinationColor; break;
                    case "dstalpha":
                    case "dst_alpha":
                    case "dst-aplha":
                    case "destination_alpha":
                    case "destination-alpha":
                    case "destinationalpha": blend = Blend.DestinationAlpha; break;
                    case "blend":
                    case "blend_factor":
                    case "blend-factor":
                    case "blendfactor": blend = Blend.BlendFactor; break;
                    case "inv_blend_factor":
                    case "inv-blend-factor":
                    case "inverse_blend_factor":
                    case "inverse-blend-factor":
                    case "inverseblendfactor": blend = Blend.InverseBlendFactor; break;
                    case "invdstcolor":
                    case "inv_dst_color":
                    case "inv-dst-color":
                    case "invdestinationcolor":
                    case "inv_destination_color":
                    case "inv-destination-color":
                    case "inverse_destination_color":
                    case "inverse-destination-color":
                    case "inversedestinationcolor": blend = Blend.InverseDestinationColor; break;
                    case "invdstalpha":
                    case "inv_dst_alpha":
                    case "inv-dst-alpha":
                    case "invdestinationalpha":
                    case "inv_destination_alpha":
                    case "inv-destination-alpha":
                    case "inverse_destination_alpha":
                    case "inverse-destination-alpha":
                    case "inversedestinationalpha": blend = Blend.InverseDestinationAlpha; break;
                }
            }
        }

        private void processColorChannels(ref ColorWriteChannels channels, JsonObject jo, params string[] args)
        {
            var jv = findFirst(jo, args);

            if (jv.Type == JsonTypes.Array)
            {
                var ja = (JsonArray)jv;
                for (var i = 0; i < ja.Count; i++)
                    _processColorChannels(ref channels, ja[i]);
            }
            else
                _processColorChannels(ref channels, jv);
        }

        private void _processColorChannels(ref ColorWriteChannels channels, JsonValue jv)
        {
            if (jv.Type == JsonTypes.Number)
            {
                var num = ((int)jv) & 0xf;
                channels |= (ColorWriteChannels)num;
            }
            else if (jv.Type == JsonTypes.String)
            {
                var name = (string)jv;
                switch (name.ToLower())
                {
                    case "none": channels |= ColorWriteChannels.None; break;
                    case "red": channels |= ColorWriteChannels.Red; break;
                    case "blue": channels |= ColorWriteChannels.Blue; break;
                    case "green": channels |= ColorWriteChannels.Green; break;
                    case "alpha": channels |= ColorWriteChannels.Alpha; break;
                }
            }
        }

        private void processColor(ref Color color, JsonObject jo, params string[] args)
        {
            var jv = findFirst(jo, args);

            if (jv.Type == JsonTypes.Number)
            {
                var num = Utility.Clamp((float)jv, 1f, 0f);
                color = new Color(num, num, num, 1f);
            }
            else if (jv.Type == JsonTypes.Object)
            {
                var j = (JsonObject)jv;
                var jr = j["r"];
                var jg = j["g"];
                var jb = j["b"];
                var ja = j["a"];

                if (jr.Type == JsonTypes.Number)
                    color.r = Utility.Clamp((float)jr, 1f, 0f);

                if (jg.Type == JsonTypes.Number)
                    color.g = Utility.Clamp((float)jg, 1f, 0f);

                if (jb.Type == JsonTypes.Number)
                    color.b = Utility.Clamp((float)jb, 1f, 0f);

                if (ja.Type == JsonTypes.Number)
                    color.a = Utility.Clamp((float)ja, 1f, 0f);
            }
            else if (jv.Type == JsonTypes.Array)
            {
                var a = (JsonArray)jv;
                var jr = a.Count < 1 ? JsonNull.Null : a[0];
                var jg = a.Count < 2 ? JsonNull.Null : a[1];
                var jb = a.Count < 3 ? JsonNull.Null : a[2];
                var ja = a.Count < 4 ? JsonNull.Null : a[3];

                if (jr.Type == JsonTypes.Number)
                    color.r = Utility.Clamp((float)jr, 1f, 0f);

                if (jg.Type == JsonTypes.Number)
                    color.g = Utility.Clamp((float)jg, 1f, 0f);

                if (jb.Type == JsonTypes.Number)
                    color.b = Utility.Clamp((float)jb, 1f, 0f);

                if (ja.Type == JsonTypes.Number)
                    color.a = Utility.Clamp((float)ja, 1f, 0f);
            }
        }

        private void processBool(ref bool b, JsonObject jo, params string[] args)
        {
            var jv = findFirst(jo, args);

            if (jv.Type == JsonTypes.Number)
            {
                b = Utility.Clamp((int)jv, 1, 0) == 1;
            }
            else if (jv.Type == JsonTypes.String)
            {
                switch (((string)jv).ToLower())
                {
                    case "0":
                    case "false": b = false; break;
                    case "1":
                    case "true": b = true; break;
                }
            }
            else if (jv.Type == JsonTypes.Bool)
            {
                b = (bool)jv;
            }
        }

        private void processInt(ref int num, JsonObject jo, int min, int max, params string[] args)
        {
            var jv = findFirst(jo, args);

            if (jv.Type == JsonTypes.Number)
            {
                num = Utility.Clamp((int)jv, min, max);
            }
        }

        private void processFloat(ref float num, JsonObject jo, float min, float max, params string[] args)
        {
            var jv = findFirst(jo, args);

            if (jv.Type == JsonTypes.Number)
            {
                num = Utility.Clamp((float)jv, min, max);
            }
        }

        private void processBlendState(ref BlendState state, JsonValue jv)
        {
            if (jv.Type == JsonTypes.String)
            {
                var blendName = (string)jv;
                switch (blendName)
                {
                    case "additive": state = BlendState.additive; break;
                    case "alpha":
                    case "alphablend": state = BlendState.alphaBlend; break;
                    case "nonpremultiple":
                    case "nonpremultiplied": state = BlendState.nonPremultiplied; break;
                    case "opaque": state = BlendState.opaque; break;
                }
            }
            else if (jv.Type == JsonTypes.Object)
            {
                var jo = (JsonObject)jv;
                processBlendFunction(ref state.AlphaBlendFunction, jo, "alpha", "alpha_blend", "alpha_func");
                processBlendFunction(ref state.ColorBlendFunction, jo, "color", "color_blend", "color_func");
                processBlend(ref state.AlphaDestinationBlend, jo, "alpha_dst");
                processBlend(ref state.AlphaSourceBlend, jo, "alpha_src");
                processBlend(ref state.ColorDestinationBlend, jo, "color_dst");
                processBlend(ref state.ColorSourceBlend, jo, "color_src");
                processColorChannels(ref state.ColorWriteChannels, jo, "write_chan", "write_channels", "color_write", "color_write_chan", "color_write_channels");
                processColor(ref state.BlendFactor, jo, "blend", "factor", "blend_factor");
                processInt(ref state.MultiSampleMask, jo, int.MinValue, int.MaxValue, "sample", "sample_mask", "multi_sample", "multi_sample_mask");
            }
        }


        private void processTextureMode(ref TextureAddressMode mode, JsonObject jo, params string[] args)
        {
            var jv = findFirst(jo, args);
            if (jv.Type == JsonTypes.Number)
            {
                var num = (int)jv;
                switch (num)
                {
                    case 0: mode = TextureAddressMode.Wrap; break;
                    case 1: mode = TextureAddressMode.Clamp; break;
                    case 2: mode = TextureAddressMode.Mirror; break;
                }
            }
            else if (jv.Type == JsonTypes.String)
            {
                var name = (string)jv;
                switch (name.ToLower())
                {
                    case "0":
                    case "wrap": mode = TextureAddressMode.Wrap; break;
                    case "1":
                    case "clamp": mode = TextureAddressMode.Clamp; break;
                    case "2":
                    case "mirror": mode = TextureAddressMode.Mirror; break;
                }
            }
        }

        private void processTextureFilter(ref TextureFilter filter, JsonObject jo, params string[] args)
        {
            var jv = findFirst(jo, args);
            if (jv.Type == JsonTypes.Number)
            {
                var num = (int)jv;
                switch (num)
                {
                    case 0: filter = TextureFilter.Linear; break;
                    case 1: filter = TextureFilter.Point; break;
                    case 2: filter = TextureFilter.Anisotropic; break;
                }
            }
            else if (jv.Type == JsonTypes.String)
            {
                var name = (string)jv;
                switch (name.ToLower())
                {
                    case "0":
                    case "linear": filter = TextureFilter.Linear; break;
                    case "1":
                    case "point": filter = TextureFilter.Point; break;
                    case "2":
                    case "anis":
                    case "anisotropic": filter = TextureFilter.Anisotropic; break;
                    case "linear_mip_point": filter = TextureFilter.LinearMipPoint; break;
                    case "point_mip_linear": filter = TextureFilter.PointMipLinear; break;
                    case "point_linear":
                    case "min_linear_mag_point_mip_linear": filter = TextureFilter.MinLinearMagPointMipLinear; break;
                    case "point_point":
                    case "min_linear_mag_point_mip_point": filter = TextureFilter.MinLinearMagPointMipPoint; break;
                    case "linear_linear":
                    case "min_point_mag_linear_mip_linear": filter = TextureFilter.MinPointMagLinearMipLinear; break;
                    case "linear_point":
                    case "min_point_mag_linear_mip_point": filter = TextureFilter.MinPointMagLinearMipPoint; break;
                }
            }
        }

        private void processSamplerState(ref SamplerState state, JsonValue jv)
        {
            if (jv.Type == JsonTypes.String)
            {
                var blendName = (string)jv;
                switch (blendName)
                {
                    case "anis_wrap":
                    case "anisotropic_wrap": state = SamplerState.anisotropicWrap; break;
                    case "anis_clamp":
                    case "anisotropic_clamp": state = SamplerState.anisotropicClamp; break;
                    case "linear_wrap": state = SamplerState.linearWrap; break;
                    case "linear_clamp": state = SamplerState.linearClamp; break;
                    case "point_wrap": state = SamplerState.pointWrap; break;
                    case "point_clamp": state = SamplerState.pointClamp; break;
                }
            }
            else if (jv.Type == JsonTypes.Object)
            {
                var jo = (JsonObject)jv;
                processTextureMode(ref state.AddressU, jo, "u", "addr_u", "tex_addr_u", "texture_address_u", "address_u");
                processTextureMode(ref state.AddressV, jo, "v", "addr_v", "tex_addr_v", "texture_address_v", "address_v");
                processTextureMode(ref state.AddressW, jo, "w", "addr_w", "tex_addr_w", "texture_address_w", "address_w");
                processTextureFilter(ref state.Filter, jo, "filter", "tex_filter", "texture_filter");
                processInt(ref state.MaxAnisotropy, jo, 0, int.MaxValue, "anis", "max_anis", "anisotropy", "max_anisotropy");
                processInt(ref state.MaxMipLevel, jo, 0, int.MaxValue, "mip", "max_mip", "max_mip_level");
                processFloat(ref state.MipMapLevelOfDetailBias, jo, 0f, float.MaxValue, "bias", "mip_bias", "max_mip_bias");
            }

        }


        private void processCullMode(ref CullMode mode, JsonObject jo, params string[] args)
        {
            var jv = findFirst(jo, args);
            if (jv.Type == JsonTypes.Number)
            {
                var num = (int)jv;
                switch (num)
                {
                    case 0: mode = CullMode.None; break;
                    case 1: mode = CullMode.CullClockwiseFace; break;
                    case 2: mode = CullMode.CullCounterClockwiseFace; break;
                }
            }
            else if (jv.Type == JsonTypes.String)
            {
                var name = (string)jv;
                switch (name.ToLower())
                {
                    case "0":
                    case "none": mode = CullMode.None; break;
                    case "1":
                    case "clockwise": mode = CullMode.CullClockwiseFace; break;
                    case "2":
                    case "counter":
                    case "counter_clockwide": mode = CullMode.CullCounterClockwiseFace; break;
                }
            }
        }

        private void processFillMode(ref FillMode mode, JsonObject jo, params string[] args)
        {
            var jv = findFirst(jo, args);
            if (jv.Type == JsonTypes.Number)
            {
                var num = (int)jv;
                switch (num)
                {
                    case 0: mode = FillMode.Solid; break;
                    case 1: mode = FillMode.WireFrame; break;
                }
            }
            else if (jv.Type == JsonTypes.String)
            {
                var name = (string)jv;
                switch (name.ToLower())
                {
                    case "0":
                    case "solid": mode = FillMode.Solid; break;
                    case "1":
                    case "wire":
                    case "wire_frame": mode = FillMode.WireFrame; break;
                }
            }
        }

        private void processRasterizerState(ref RasterizerState state, JsonValue jv)
        {
            if (jv.Type == JsonTypes.String)
            {
                var blendName = (string)jv;
                switch (blendName)
                {
                    case "none": state = RasterizerState.none; break;
                    case "clockwise": state = RasterizerState.clockwise; break;
                    case "counter":
                    case "counter_clockwise": state = RasterizerState.counterClockwise; break;
                }
            }
            else if (jv.Type == JsonTypes.Object)
            {
                var jo = (JsonObject)jv;
                processCullMode(ref state.CullMode, jo, "cull", "cull_mode");
                processFillMode(ref state.FillMode, jo, "fill", "fill_mode");
                processBool(ref state.MultiSampleAntiAlias, jo, "alias", "anti_alias", "sample_anti_alias", "multi_sample_anti_alias");
                processBool(ref state.ScissorTestEnable, jo, "scissor", "scissor_test", "scissor_test_enabled");
                processFloat(ref state.DepthBias, jo, 0f, float.MaxValue, "bias", "depth_bias", "depth");
                processFloat(ref state.SlopeScaleDepthBias, jo, 0f, float.MaxValue, "slope_bias", "slope_depth", "slope");
            }
        }

        private void processStencilOperation(ref StencilOperation op, JsonObject jo, params string[] args)
        {
            var jv = findFirst(jo, args);
            if (jv.Type == JsonTypes.Number)
            {
                var num = (int)jv;
                switch (num)
                {
                    case 0: op = StencilOperation.Keep; break;
                    case 1: op = StencilOperation.Zero; break;
                    case 2: op = StencilOperation.Replace; break;
                    case 3: op = StencilOperation.Increment; break;
                    case 4: op = StencilOperation.Decrement; break;
                    case 5: op = StencilOperation.IncrementSaturation; break;
                    case 6: op = StencilOperation.DecrementSaturation; break;
                    case 7: op = StencilOperation.Invert; break;
                }
            }
            else if (jv.Type == JsonTypes.String)
            {
                var name = (string)jv;
                switch (name.ToLower())
                {
                    case "0":
                    case "keep": op = StencilOperation.Keep; break;
                    case "1":
                    case "zero": op = StencilOperation.Zero; break;
                    case "2":
                    case "replace": op = StencilOperation.Replace; break;
                    case "3":
                    case "inc":
                    case "increment": op = StencilOperation.Increment; break;
                    case "4":
                    case "dec":
                    case "decrement": op = StencilOperation.Decrement; break;
                    case "5":
                    case "inc_sat":
                    case "inc_saturation":
                    case "increment_sat":
                    case "increment_saturation": op = StencilOperation.IncrementSaturation; break;
                    case "6":
                    case "dec_sat":
                    case "dec_saturation":
                    case "decrement_sat":
                    case "decrement_saturation": op = StencilOperation.DecrementSaturation; break;
                    case "7":
                    case "inv":
                    case "invert": op = StencilOperation.Invert; break;
                }
            }
        }

        private void processCompareFunction(ref CompareFunction cf, JsonObject jo, params string[] args)
        {
            var jv = findFirst(jo, args);
            if (jv.Type == JsonTypes.Number)
            {
                var num = (int)jv;
                switch (num)
                {
                    case 0: cf = CompareFunction.Always; break;
                    case 1: cf = CompareFunction.Never; break;
                    case 2: cf = CompareFunction.Less; break;
                    case 3: cf = CompareFunction.LessEqual; break;
                    case 4: cf = CompareFunction.Equal; break;
                    case 5: cf = CompareFunction.GreaterEqual; break;
                    case 6: cf = CompareFunction.Greater; break;
                    case 7: cf = CompareFunction.NotEqual; break;
                }
            }
            else if (jv.Type == JsonTypes.String)
            {
                var name = (string)jv;
                switch (name.ToLower())
                {
                    case "0":
                    case "always": cf = CompareFunction.Always; break;
                    case "1":
                    case "never": cf = CompareFunction.Never; break;
                    case "2":
                    case "ls":
                    case "less": cf = CompareFunction.Less; break;
                    case "3":
                    case "ls_eq":
                    case "ls_equal":
                    case "less_eq":
                    case "less_equal": cf = CompareFunction.LessEqual; break;
                    case "4":
                    case "eq":
                    case "equal": cf = CompareFunction.Equal; break;
                    case "5":
                    case "gr_eq":
                    case "gr_equal":
                    case "greater_eq":
                    case "greater_equal": cf = CompareFunction.GreaterEqual; break;
                    case "6":
                    case "gr":
                    case "greater": cf = CompareFunction.Greater; break;
                    case "7":
                    case "not_eq":
                    case "not_equal": cf = CompareFunction.NotEqual; break;
                }
            }
        }

        private void processDepthStencilState(ref DepthStencilState state, JsonValue jv)
        {

            if (jv.Type == JsonTypes.String)
            {
                var blendName = (string)jv;
                switch (blendName)
                {
                    case "default":
                    case "normal": state = DepthStencilState._default; break;
                    case "read": state = DepthStencilState.read; break;
                    case "none": state = DepthStencilState.none; break;
                }
            }
            else if (jv.Type == JsonTypes.Object)
            {
                var jo = (JsonObject)jv;
                processStencilOperation(ref state.CounterClockwiseStencilDepthBufferFail, jo, "counter_depth_fail");
                processStencilOperation(ref state.CounterClockwiseStencilFail, jo, "counter_fail");
                processStencilOperation(ref state.CounterClockwiseStencilPass, jo, "counter_pass");
                processStencilOperation(ref state.StencilDepthBufferFail, jo, "depth_fail");
                processStencilOperation(ref state.StencilPass, jo, "pass");
                processStencilOperation(ref state.StencilFail, jo, "fail");
                processCompareFunction(ref state.CounterClockwiseStencilFunction, jo, "counter_func", "counter_stencil_func");
                processCompareFunction(ref state.StencilFunction, jo, "func", "stencil_func");
                processCompareFunction(ref state.DepthBufferFunction, jo, "depth_func", "depth_buffer_func");
                processInt(ref state.ReferenceStencil, jo, int.MinValue, int.MaxValue, "ref_stencil", "ref");
                processInt(ref state.StencilWriteMask, jo, int.MinValue, int.MaxValue, "write_mask", "stencil_write_mask");
                processInt(ref state.StencilMask, jo, int.MinValue, int.MaxValue, "mask", "stencil_mask");
                processBool(ref state.DepthBufferEnable, jo, "buffer", "buffer_enabled", "depth_enabled", "depth_buffer_enabled");
                processBool(ref state.DepthBufferWriteEnable, jo, "two_sided", "two_sided_mode", "two_sided_stencil_mode");
                processBool(ref state.TwoSidedStencilMode, jo, "stencil", "stencil_enabled");
                processBool(ref state.StencilEnable, jo, "scissor", "scissor_test", "scissor_test_enabled");
            }
        }
        #endregion

    }
}
