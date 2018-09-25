namespace RaspPiTest.FritzBox.Model
{
    public enum ThermostatErrorCode
    {
        //0: kein Fehler
        NoError,
        //1: Keine Adaptierung möglich. Gerät korrekt am Heizkörper montiert?
        InstallationError,
        //2: Ventilhub zu kurz oder Batterieleistung zu schwach. Ventilstößel per Hand mehrmals öfnen und schließen oder
        ValveError,
        //neue Batterien einsetzen.
        BatteryEmptyError,
        //3: Keine Ventilbewegung möglich. Ventilstößel frei?
        ValveBlockedError,
        //4: Die Installation wird gerade vorbereitet.
        PreinstallationInProgressError,
        //5: Der Heizkörperregler ist im Installationsmodus und kann auf das Heizungsventil montiert werden.
        InstallationInProgressError,
        //6: Der Heizkörperregler passt sich nun an den Hub des Heizungsventils an.
        SetupInProgressError
    }
}