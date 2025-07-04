﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConsoleTool {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ConsoleTool.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No assembly file specified. Use the /file switch and try sigexp /? for complete usage info..
        /// </summary>
        internal static string ERROR_AssemblyFileNotSpecified {
            get {
                return ResourceManager.GetString("ERROR_AssemblyFileNotSpecified", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Method &apos;{0}&apos; in type &apos;{1}&apos; is neither a P/Invoke nor an RCW method. Native signatures can only be generated for interop methods..
        /// </summary>
        internal static string ERROR_MethodIsNotInterop {
            get {
                return ResourceManager.GetString("ERROR_MethodIsNotInterop", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to find method &apos;{0}&apos; in type &apos;{1}&apos;..
        /// </summary>
        internal static string ERROR_UnableToFindMethod {
            get {
                return ResourceManager.GetString("ERROR_UnableToFindMethod", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to load assembly file &apos;{0}&apos;: &apos;{1}&apos;..
        /// </summary>
        internal static string ERROR_UnableToLoadAssembly {
            get {
                return ResourceManager.GetString("ERROR_UnableToLoadAssembly", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to load type &apos;{0}&apos;: &apos;{1}&apos;..
        /// </summary>
        internal static string ERROR_UnableToLoadType {
            get {
                return ResourceManager.GetString("ERROR_UnableToLoadType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unrecognized command line option &apos;{0}&apos;. Try sigexp /?..
        /// </summary>
        internal static string ERROR_UnrecognizedOption {
            get {
                return ResourceManager.GetString("ERROR_UnrecognizedOption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Usage: sigexp [&lt;options&gt;] /file &lt;path_to_assembly&gt; [&lt;def_1&gt;,&lt;def_2&gt;, ...]
        ///
        ///    /32         - generate signatures for 32-bit target platform (Default)
        ///    /64         - generate signatures for 64-bit target platform
        ///
        ///    /unicode    - generate signatures for Unicode target platform (Default)
        ///    /ansi       - generate signatures for ANSI target platform - Win9x
        ///
        ///    /wintypes   - use standard Windows types like &apos;LPCSTR&apos; (Default)
        ///    /plaintypes - use plain C++ types like &apos;const char *&apos;
        ///
        ///    /colo [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string INFO_CommandLineHelp {
            get {
                return ResourceManager.GetString("INFO_CommandLineHelp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Microsoft (R) Managed to Unmanaged Signature Converter version {0}
        ///Copyright (C) Microsoft Corporation.  All rights reserved.
        ///.
        /// </summary>
        internal static string INFO_CommandLineLogo {
            get {
                return ResourceManager.GetString("INFO_CommandLineLogo", resourceCulture);
            }
        }
    }
}
