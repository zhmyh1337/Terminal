﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FileManager.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Localization {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Localization() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("FileManager.Properties.Localization", typeof(Localization).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This command changes the disk if specified, otherwise displays the list of all disks..
        /// </summary>
        public static string cmdDisk {
            get {
                return ResourceManager.GetString("cmdDisk", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Disk to set (either its ID or name)..
        /// </summary>
        public static string diskDisk {
            get {
                return ResourceManager.GetString("diskDisk", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to find the disk with name &quot;{0}&quot;..
        /// </summary>
        public static string eDiskInvalidName {
            get {
                return ResourceManager.GetString("eDiskInvalidName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to find the disk with number {0}..
        /// </summary>
        public static string eDiskInvalidNumber {
            get {
                return ResourceManager.GetString("eDiskInvalidNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;--version&quot; option is invalid in this context..
        /// </summary>
        public static string eVersionAsOption {
            get {
                return ResourceManager.GetString("eVersionAsOption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Suppresses summary message and stops executing after the command completes..
        /// </summary>
        public static string quiteQuite {
            get {
                return ResourceManager.GetString("quiteQuite", resourceCulture);
            }
        }
    }
}
