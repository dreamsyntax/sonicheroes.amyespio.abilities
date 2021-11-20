using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using sonicheroes.amyespio.abilities.Configuration;
using sonicheroes.amyespio.abilities.Configuration.Implementation;
using System;
using Reloaded.Memory.Sources;
using Reloaded.Mod.Interfaces.Internal;

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
        private IModLoader _modLoader;

        /// <summary>
        /// Stores the contents of your mod's configuration. Automatically updated by template.
        /// </summary>
        private Config _configuration;

        /// <summary>
        /// An interface to Reloaded's the function hooks/detours library.
        /// See: https://github.com/Reloaded-Project/Reloaded.Hooks
        ///      for documentation and samples. 
        /// </summary>
        private IReloadedHooks _hooks;

        /// <summary>
        /// Entry point for your mod.
        /// </summary>
        public void Start(IModLoaderV2 loader)
        {
            _modLoader = (IModLoader)loader;
            _logger = (ILogger)_modLoader.GetLogger();
            _modLoader.GetController<IReloadedHooks>().TryGetTarget(out _hooks);

            // Your config file is in Config.json.
            // Need a different name, format or more configurations? Modify the `Configurator`.
            // If you do not want a config, remove Configuration folder and Config class.
            var configurator = new Configurator(_modLoader.GetDirectoryForModId(MyModId));
            _configuration = configurator.GetConfiguration<Config>(0);
            _configuration.ConfigurationUpdated += OnConfigurationUpdated;

            /* Your mod code starts here. */

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
            Memory.Instance.SafeWrite((IntPtr)0x005A67C0, new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0xEB });
            Memory.Instance.SafeWrite<byte>((IntPtr)0x005AF714, 0x7A);
            Memory.Instance.SafeWrite<byte>((IntPtr)0x005AF720, 0x7A);
            Memory.Instance.SafeWrite<byte>((IntPtr)0x005CFAEE, 0x7F);
        }

        private void DisableAmyEspioAbilities()
        {
            Memory.Instance.SafeWrite((IntPtr)0x005A67C0, new byte[] { 0x8A, 0x88, 0xBB, 0x00, 0x00, 0x00, 0x84, 0xC9, 0x74 });
            Memory.Instance.SafeWrite<byte>((IntPtr)0x005AF714, 0x87);
            Memory.Instance.SafeWrite<byte>((IntPtr)0x005AF720, 0x87);
            Memory.Instance.SafeWrite<byte>((IntPtr)0x005CFAEE, 0x06);
        }

        private void OnConfigurationUpdated(IConfigurable obj)
        {

        /* This is executed when the configuration file gets updated by the user
                at runtime.*/


            // Replace configuration with new.
            _configuration = (Config)obj;
            _logger.WriteLine($"[{MyModId}] Config Updated: Applying");

            // Apply settings from configuration.
            // ... your code here.
        }

        /* Mod loader actions. */
        public void Suspend()
        {
            DisableAmyEspioAbilities();
            /*  Some tips if you wish to support this (CanSuspend == true)
             
                A. Undo memory modifications.
                B. Deactivate hooks. (Reloaded.Hooks Supports This!)
            */
        }

        public void Resume()
        {
            EnableAmyEspioAbilities();
            /*  Some tips if you wish to support this (CanSuspend == true)
             
                A. Redo memory modifications.
                B. Re-activate hooks. (Reloaded.Hooks Supports This!)
            */
        }

        public void Unload()
        {
            DisableAmyEspioAbilities();
            /*  Some tips if you wish to support this (CanUnload == true).
             
                A. Execute Suspend(). [Suspend should be reusable in this method]
                B. Release any unmanaged resources, e.g. Native memory.
            */
        }

        /*  If CanSuspend == false, suspend and resume button are disabled in Launcher and Suspend()/Resume() will never be called.
            If CanUnload == false, unload button is disabled in Launcher and Unload() will never be called.
        */
        public bool CanUnload() => true;
        public bool CanSuspend() => true;

        /* Automatically called by the mod loader when the mod is about to be unloaded. */
        public Action Disposing { get; }

        /* Contains the Types you would like to share with other mods.
           If you do not want to share any types, please remove this method and the
           IExports interface.
        
           Inter Mod Communication: https://github.com/Reloaded-Project/Reloaded-II/blob/master/Docs/InterModCommunication.md
        */
        public Type[] GetTypes() => new Type[0];

        /* This is a dummy for R2R (ReadyToRun) deployment.
           For more details see: https://github.com/Reloaded-Project/Reloaded-II/blob/master/Docs/ReadyToRun.md
        */
        public static void Main() { }
    }
}
