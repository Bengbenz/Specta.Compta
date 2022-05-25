namespace Beng.Specta.Compta.Core.Objects.Identities;

/// <summary>
/// This is an example of how you would manage what optional parts of your system a user can access
/// NOTE: You can add Display attributes (as done on Permissions) to give more information about a module
/// </summary>
[Flags]
public enum PaidForModule : long
{
    None = 0,
    Dashboard,
    Project,
    Funding,
    Budget,
    Accounting,
    Treasury,
    Admin
}