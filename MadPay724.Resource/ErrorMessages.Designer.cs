﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MadPay724.Resource {
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
    public class ErrorMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ErrorMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MadPay724.Resource.ErrorMessages", typeof(ErrorMessages).Assembly);
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
        ///   Looks up a localized string similar to user was not register in db.
        /// </summary>
        public static string DbErrorRegister {
            get {
                return ResourceManager.GetString("DbErrorRegister", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to error.
        /// </summary>
        public static string Error {
            get {
                return ResourceManager.GetString("Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to this UserName alredy exist !.
        /// </summary>
        public static string ExistUserMessage {
            get {
                return ResourceManager.GetString("ExistUserMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to password dos not changed !.
        /// </summary>
        public static string NoChangedPassword {
            get {
                return ResourceManager.GetString("NoChangedPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Regiser was not done..
        /// </summary>
        public static string NoRegister {
            get {
                return ResourceManager.GetString("NoRegister", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to there is no file to upload.
        /// </summary>
        public static string NotExistFile {
            get {
                return ResourceManager.GetString("NotExistFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to legnth of password most be between 8 to 16 character.
        /// </summary>
        public static string PasswordLengthMessage {
            get {
                return ResourceManager.GetString("PasswordLengthMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Email is not correct.
        /// </summary>
        public static string WrogEmailFormatMessage {
            get {
                return ResourceManager.GetString("WrogEmailFormatMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to username or password is wrong !.
        /// </summary>
        public static string WrongEmailOrPassword {
            get {
                return ResourceManager.GetString("WrongEmailOrPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to password is wong !.
        /// </summary>
        public static string WrongPassword {
            get {
                return ResourceManager.GetString("WrongPassword", resourceCulture);
            }
        }
    }
}
