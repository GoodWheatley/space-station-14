namespace Content.Server.Remotes
{
    [RegisterComponent]
    [Access(typeof(DoorRemoteSystem))]
    public sealed class DoorRemoteComponent : Component
    {
        public OperatingMode Mode = OperatingMode.OpenClose;

        public enum OperatingMode : byte
        {
            OpenClose,
            ToggleBolts,
            ToggleEmergencyAccess
        }
        
    }
    
    [DataField("connectionRequired")]
    public float connectionRequired = false
    
    [DataField("connectTime")]
    public float connectTime = 3f;
}
