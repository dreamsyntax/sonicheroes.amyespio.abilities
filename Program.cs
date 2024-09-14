using Reloaded.Memory.Interfaces;
using Reloaded.Mod.Interfaces;
using Reloaded.Mod.Interfaces.Internal;
using System;
using Memory = Reloaded.Memory.Memory;

namespace sonicheroes.amyespio.abilities
{
    public class Program : IMod
    {
        /// <summary>
        /// Your mod if from ModConfig.json, used during initialization.
        /// </summary>
        private const string MyModId = "sonicheroes.amyespio.abilities";

        /// <summary>
        /// Used for writing text to the console window.
        /// </summary>
        private ILogger _logger;

        /// <summary>
        /// Provides access to the mod loader API.
        /// </summary>
        private IModLoader _loader;


        /// <summary>
        /// Entry point for your mod.
        /// </summary>
        public void Start(IModLoaderV1 loader)
        {
            _loader = (IModLoader)loader;
            _logger = (ILogger)_loader.GetLogger();

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
            Memory.Instance.SafeWrite(0x005A67C0, [0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0xEB]);
            Memory.Instance.SafeWrite(0x005AF714, [0x7A]);
            Memory.Instance.SafeWrite(0x005AF720, [0x7A]);
            Memory.Instance.SafeWrite(0x005CFAEE, [0x7F]);
        }

        private void DisableAmyEspioAbilities()
        {
            Memory.Instance.SafeWrite(0x005A67C0, [0x8A, 0x88, 0xBB, 0x00, 0x00, 0x00, 0x84, 0xC9, 0x74]);
            Memory.Instance.SafeWrite(0x005AF714, [0x87]);
            Memory.Instance.SafeWrite(0x005AF720, [0x87]);
            Memory.Instance.SafeWrite(0x005CFAEE, [0x06]);
        }

        public void Suspend() => DisableAmyEspioAbilities();
        public void Resume() => EnableAmyEspioAbilities();
        public void Unload() => Suspend();

        public bool CanUnload() => true;
        public bool CanSuspend() => true;

        /* Automatically called by the mod loader when the mod is about to be unloaded. */
        public Action Disposing { get; }

        Action IModV1.Disposing => throw new NotImplementedException();

        /* This is a dummy for R2R (ReadyToRun) deployment.
           For more details see: https://github.com/Reloaded-Project/Reloaded-II/blob/master/Docs/ReadyToRun.md
        */
        public static void Main() { }
    }
}
