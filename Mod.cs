using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Memory.Interfaces;
using Reloaded.Mod.Interfaces;
using sonicheroes.amyespio.abilities.Template;
using Memory = Reloaded.Memory.Memory;

namespace sonicheroes.amyespio.abilities
{
    /// <summary>
    /// Your mod logic goes here.
    /// </summary>
    public class Mod : ModBase // <= Do not Remove.
    {
        /// <summary>
        /// Provides access to the mod loader API.
        /// </summary>
        private readonly IModLoader _modLoader;

        /// <summary>
        /// Provides access to the Reloaded.Hooks API.
        /// </summary>
        /// <remarks>This is null if you remove dependency on Reloaded.SharedLib.Hooks in your mod.</remarks>
        private readonly IReloadedHooks? _hooks;

        /// <summary>
        /// Provides access to the Reloaded logger.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Entry point into the mod, instance that created this class.
        /// </summary>
        private readonly IMod _owner;

        /// <summary>
        /// The configuration of the currently executing mod.
        /// </summary>
        private readonly IModConfig _modConfig;

        public Mod(ModContext context)
        {
            _modLoader = context.ModLoader;
            _hooks = context.Hooks;
            _logger = context.Logger;
            _owner = context.Owner;
            _modConfig = context.ModConfig;

            // Original Hex Edit credit to SCHG: Sonic Heroes / Muzzarino
            // https://info.sonicretro.org/SCHG:Sonic_Heroes/EXE_Editing

            // sec1: 001A67C0
            // original:   8A 88 BB 00 00 00 84 C9 74
            // hacked:     90 90 90 90 90 90 90 90 EB

            // sec2: 001AF714
            // original: 0x87
            // hacked:   0x7A

            // sec3: 001AF720
            // original: 0x87
            // hacked:   0x7A

            // sec4: 001CFAEE
            // original: 0x06
            // hacked:   0x7F

            EnableAmyEspioAbilities();
        }

        private void EnableAmyEspioAbilities()
        {
            _logger.WriteLineAsync("Amy/Espio Abilities Enabled");
            Memory.Instance.SafeWrite(0x005A67C0, [0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0xEB]);
            Memory.Instance.SafeWrite(0x005AF714, [0x7A]);
            Memory.Instance.SafeWrite(0x005AF720, [0x7A]);
            Memory.Instance.SafeWrite(0x005CFAEE, [0x7F]);
        }

        private void DisableAmyEspioAbilities()
        {
            _logger.WriteLineAsync("Amy/Espio Abilities Disabled");
            Memory.Instance.SafeWrite(0x005A67C0, [0x8A, 0x88, 0xBB, 0x00, 0x00, 0x00, 0x84, 0xC9, 0x74]);
            Memory.Instance.SafeWrite(0x005AF714, [0x87]);
            Memory.Instance.SafeWrite(0x005AF720, [0x87]);
            Memory.Instance.SafeWrite(0x005CFAEE, [0x06]);
        }

        public override void Suspend() => DisableAmyEspioAbilities();
        public override void Resume() => EnableAmyEspioAbilities();
        public override void Unload() => Suspend();

        public override bool CanUnload() => true;
        public override bool CanSuspend() => true;

        #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Mod() { }
#pragma warning restore CS8618
        #endregion
    }
}