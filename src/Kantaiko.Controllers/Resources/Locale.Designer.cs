﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kantaiko.Controllers.Resources {
    using System;


    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Locale {

        private static global::System.Resources.ResourceManager resourceMan;

        private static global::System.Globalization.CultureInfo resourceCulture;

        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Locale() {
        }

        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Kantaiko.Controllers.Resources.Locale", typeof(Locale).Assembly);
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
        ///   Looks up a localized string similar to Bool required.
        /// </summary>
        internal static string BoolRequired {
            get {
                return ResourceManager.GetString("BoolRequired", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Integer required.
        /// </summary>
        internal static string IntegerRequired {
            get {
                return ResourceManager.GetString("IntegerRequired", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Missing required parameter {0}.
        /// </summary>
        internal static string MissingRequiredParameter {
            get {
                return ResourceManager.GetString("MissingRequiredParameter", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Number required.
        /// </summary>
        internal static string NumberRequired {
            get {
                return ResourceManager.GetString("NumberRequired", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Should be no less than {0}.
        /// </summary>
        internal static string ShouldBeNoLessThan {
            get {
                return ResourceManager.GetString("ShouldBeNoLessThan", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Should be no more than {0}.
        /// </summary>
        internal static string ShouldBeNoMoreThan {
            get {
                return ResourceManager.GetString("ShouldBeNoMoreThan", resourceCulture);
            }
        }
    }
}
