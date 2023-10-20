using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeEventChannelContainer : Singleton<RuntimeEventChannelContainer>
{
    public EventChannelAbilities EventChannelAbilitiesInstance;
    public EventChannelAudioControl EventChannelAudioControlInstance;
    public EventChannelAudioMix EventChannelAudioMixInstance;
    public EventChannelDespairScoreTick EventChannelDespairScoreTickInstance;
    public EventChannelGameFlow EventChannelGameFlowInstance;
    public EventChannelLevelUp EventChannelLevelUpInstance;
    public EventChannelScanInfo EventChannelScanInfoInstance;
    public EventChannelShipSpawner EventChannelShipSpawnerInstance;
    public EventChannelSunlightControl EventChannelSunlightControlInstance;

}
