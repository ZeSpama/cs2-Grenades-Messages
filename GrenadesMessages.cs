using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace GrenadesMessages {
  public class GrenadesMessages: BasePlugin {
    public override string ModuleName => "GrenadesMessages";
    public override string ModuleVersion => "0.0.2";


    public override void Load(bool hotReload)
    {
      RegisterEventHandler < EventGrenadeThrown > (OnGrenadeThrown);
      RegisterListener<Listeners.OnMapStart>(mapName =>
        {
            Server.NextFrame(() =>
            {
                Server.ExecuteCommand("sv_ignoregrenaderadio 1");
            });
        });
    }

    public HookResult OnGrenadeThrown(EventGrenadeThrown @event, GameEventInfo info) {
      CCSPlayerController ? player = @event.Userid;

      if (player == null) {
        return HookResult.Continue;
      }

      string grenadename = @event.Weapon;

      var grenadeMessages = new Dictionary < string, string >
          {
            { "hegrenade", "ThrowHE" },
            { "smokegrenade", "ThrowSMOKE" },
            { "molotov", "ThrowMOLO" },
            { "flashbang", "ThrowFLASH" },
            { "decoy", "ThrowDECOY" },
            { "incgrenade", "ThrowINC" }
        };

      if (grenadeMessages.TryGetValue(grenadename, out string ? messageKey)) {
        Server.NextFrame(() => {
          player.PrintToChat(Localizer["Prefix"] + Localizer[messageKey]);
        });
      }

      return HookResult.Continue;
    }

  }
}