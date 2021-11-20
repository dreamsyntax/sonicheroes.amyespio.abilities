using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using Reloaded.Memory;
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
        public void Start(IModLoaderV1 loader)
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

            // address start: 001A67C0
            IntPtr sec1address = new IntPtr(0x005A67C0);
            // original:   8A 88 BB 00 00 00 84 C9 74
            // hacked:     90 90 90 90 90 90 90 90 EB
            AmyEspioData section1 = new AmyEspioData(0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0xEB);

            // address start: 001AF714
            IntPtr sec2address = new IntPtr(0x005AF714);

            // original: 0x87
            // hacked:   0x7A

            // address start: 001AF720
            IntPtr sec3address = new IntPtr(0x005AF720);

            // original: 0x87
            // hacked:   0x7A

            // address start: 001CFAEE
            IntPtr sec4address = new IntPtr(0x005CFAEE);

            // original: 0x06
            // hacked:   0x7F

            byte[] sec2hax = new byte[1];
            sec2hax[0] = 0x7A;
            Struct.ToPtr(sec1address, section1, true);

            IMemory memory = Reloaded.Memory.Sources.Memory.CurrentProcess;
            memory.SafeWriteRaw(sec2address, sec2hax);
            //Memory.Instance.SafeWriteRaw(sec2address, sec2hax);
            Memory.Instance.SafeWrite<byte>(sec3address, 0x7A);
            Memory.Instance.SafeWrite(sec4address, (byte)0x7F);
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
            /*  Some tips if you wish to support this (CanSuspend == true)
             
                A. Undo memory modifications.
                B. Deactivate hooks. (Reloaded.Hooks Supports This!)
            */
        }

        public void Resume()
        {
            /*  Some tips if you wish to support this (CanSuspend == true)
             
                A. Redo memory modifications.
                B. Re-activate hooks. (Reloaded.Hooks Supports This!)
            */
        }

        public void Unload()
        {
            /*  Some tips if you wish to support this (CanUnload == true).
             
                A. Execute Suspend(). [Suspend should be reusable in this method]
                B. Release any unmanaged resources, e.g. Native memory.
            */
        }

        /*  If CanSuspend == false, suspend and resume button are disabled in Launcher and Suspend()/Resume() will never be called.
            If CanUnload == false, unload button is disabled in Launcher and Unload() will never be called.
        */
        public bool CanUnload() => false;
        public bool CanSuspend() => false;

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
