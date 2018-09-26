namespace RaspPiTest.FritzBox
{
    /// <summary>
    /// Settings for communication with the Fritz!Box. A "fritzbox-settings.json" file must be provided. Its content
    /// should look like this:
    /// {
    ///     "connection": {
    ///         "username": "fritzboxuser",
    ///         "password":  "password"
    ///     } 
    /// }
    /// Make sure this user exists in your box and has the right to access the smart home component. Other than that, no component is needed.
    /// </summary>
    public class FritzBoxConnection
    {
        public FritzBoxConnection()
        {
            Username = Password = string.Empty;
        }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
