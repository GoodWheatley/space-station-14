using Robust.Shared.Audio;

namespace Content.Server.Flash.Components
{
    [RegisterComponent, Access(typeof(FlashSystem))]
    public sealed class FlashComponent : Component
    {
        [DataField("duration")]
        [ViewVariables(VVAccess.ReadWrite)]
        public int FlashDuration { get; set; } = 5000;

        [DataField("uses")]
        [ViewVariables(VVAccess.ReadWrite)]
        public int Uses { get; set; } = 5;

        [DataField("range")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float Range { get; set; } = 7f;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("aoeFlashDuration")]
        public int AoeFlashDuration { get; set; } = 2000;

        [DataField("slowTo")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float SlowTo { get; set; } = 0.5f;
        
        [DataField("eyeDamage")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float EyeDamage { get; set; } = 20f;
             
        [DataField("aoeEyeDamage")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float AoeEyeDamage { get; set; } = 10f;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("sound")]
        public SoundSpecifier Sound { get; set; } = new SoundPathSpecifier("/Audio/Weapons/flash.ogg");

        public bool Flashing;

        public bool HasUses => Uses > 0;
    }
}
