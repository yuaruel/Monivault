#pragma checksum "/Users/henryezeanya/Projects/DevCodes/Monivault/src/Monivault.Web.Mvc/Views/Shared/Modals/_ModalFooterWithSaveAndCancel.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4185d25ea02414165aa4dc238f2f50eab941faa1"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_Modals__ModalFooterWithSaveAndCancel), @"mvc.1.0.view", @"/Views/Shared/Modals/_ModalFooterWithSaveAndCancel.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Shared/Modals/_ModalFooterWithSaveAndCancel.cshtml", typeof(AspNetCore.Views_Shared_Modals__ModalFooterWithSaveAndCancel))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "/Users/henryezeanya/Projects/DevCodes/Monivault/src/Monivault.Web.Mvc/Views/_ViewImports.cshtml"
using Abp.Localization;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4185d25ea02414165aa4dc238f2f50eab941faa1", @"/Views/Shared/Modals/_ModalFooterWithSaveAndCancel.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f45438928397a37c72f1c7d6e067f063c536965a", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_Modals__ModalFooterWithSaveAndCancel : Monivault.Web.Views.MonivaultRazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 124, true);
            WriteLiteral("<div class=\"modal-footer\">\n    <button type=\"button\" class=\"btn btn-default close-button waves-effect\" data-dismiss=\"modal\">");
            EndContext();
            BeginContext(125, 11, false);
#line 2 "/Users/henryezeanya/Projects/DevCodes/Monivault/src/Monivault.Web.Mvc/Views/Shared/Modals/_ModalFooterWithSaveAndCancel.cshtml"
                                                                                            Write(L("Cancel"));

#line default
#line hidden
            EndContext();
            BeginContext(136, 85, true);
            WriteLiteral("</button>\n    <button type=\"button\" class=\"btn btn-primary save-button waves-effect\">");
            EndContext();
            BeginContext(222, 9, false);
#line 3 "/Users/henryezeanya/Projects/DevCodes/Monivault/src/Monivault.Web.Mvc/Views/Shared/Modals/_ModalFooterWithSaveAndCancel.cshtml"
                                                                      Write(L("Save"));

#line default
#line hidden
            EndContext();
            BeginContext(231, 17, true);
            WriteLiteral("</button>\n</div>\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
