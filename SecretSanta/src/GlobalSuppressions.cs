// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.


//we didn't want gifts to be read only.
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>", Scope = "member", Target = "~P:SecretSanta.Business.User.Gifts")]
//issue with default configure method, suppressed instead of making static to avoid possible issues a static method could make.
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1822:Member Configure does not access instance data and can be marked as static (Shared in VisualBasic)", Justification = "<Pending>")]
//another fix on the default class
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "<Pending>")]
//needs to be a string for this case and not a Uri
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1054:Change the type of parameter url of method Gift.Gift(int, string, string, string, User) from string to System.Uri, or provide an overload to Gift.Gift(int, string, string, string, User) that allows url to be passed as a System.Uri object.", Justification = "<Pending>")]
//needs to be a string and not a Uri
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1056:Change the type of property Gift.Url from string to System.Uri.", Justification = "<Pending>")]