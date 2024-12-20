﻿using PKISharp.WACS.Configuration;
using PKISharp.WACS.Plugins.Base.Options;
using PKISharp.WACS.Plugins.CsrPlugins;
using PKISharp.WACS.Services;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace PKISharp.WACS.Plugins.Base.Factories
{
    /// <summary>
    /// CsrPluginFactory base implementation
    /// </summary>
    /// <typeparam name="TPlugin"></typeparam>
    public class CsrPluginOptionsFactory<TOptions, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)] TArguments>(ArgumentsInputService arguments) :
        PluginOptionsFactory<TOptions>
        where TOptions : CsrPluginOptions, new()
        where TArguments : CsrArguments, new()
    {
        protected ArgumentResult<bool?> OcspMustStaple => arguments.
            GetBool<TArguments>(x => x.OcspMustStaple).
            WithDefault(false).
            DefaultAsNull();

        protected ArgumentResult<bool?> ReusePrivateKey => arguments.
            GetBool<TArguments>(x => x.ReusePrivateKey).
            WithDefault(false).
            DefaultAsNull();

        public override async Task<TOptions?> Default()
        {
            return new TOptions()
            {
                OcspMustStaple = await OcspMustStaple.GetValue(),
                ReusePrivateKey = await ReusePrivateKey.GetValue()
            };
        }

        public override IEnumerable<(CommandLineAttribute, object?)> Describe(TOptions options)
        {
            yield return (OcspMustStaple.Meta, options.OcspMustStaple);
            yield return (ReusePrivateKey.Meta, options.ReusePrivateKey);
        }
    }
}