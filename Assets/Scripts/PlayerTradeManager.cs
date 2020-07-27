﻿using UnityEngine;
using System.Collections;
using Mirror;

/**
 * Manages trading between players.
 * Player who is currently having a turn may be able to ask for a certain resouce.
 * Other players may send offers in exchange for that resource (one at a time for networking ease).
 * 
 * Process:
 * - A player sends a request to another player (one of the players must at least be the player who is trading)
 * - As soon as the player accepts the trade offer, the trade begins. (trading is set to true)
 * - Once the trade begins, players can now set values for how many resources they each wish to offer (setting the receiver/offerer resource values)
 * - If each player is satisfied, they can then attempt to each accept the trades (receiverAccept and offererAccept set to true)
 * - If both players are satisfied, then they must both press the trade button (receiverWantsToTrade and offererWantsToTrade set to true)
 * - The trade then resolves, and each player exchanges resources.
 */
public class PlayerTradeManager : NetworkBehaviour
{
    [SyncVar]
    public bool trading = false; // This flag is enabled if there is a trade in progress.

    [SyncVar]
    public bool receiverAccept = false;
    [SyncVar]
    public bool offererAccept = false;

    [SyncVar]
    public bool receiverWantsToTrade = false;
    [SyncVar]
    public bool offererWantsToTrade = false;

    [SyncVar]
    public string receiverId = "";
    [SyncVar]
    public int receiverLumber = 0;
    [SyncVar]
    public int receiverWool = 0;
    [SyncVar]
    public int receiverGrain = 0;
    [SyncVar]
    public int receiverBrick = 0;
    [SyncVar]
    public int receiverOre = 0;

    [SyncVar]
    public string offererId = "";
    [SyncVar]
    public int offererLumber = 0;
    [SyncVar]
    public int offererWool = 0;
    [SyncVar]
    public int offererGrain = 0;
    [SyncVar]
    public int offererBrick = 0;
    [SyncVar]
    public int offererOre = 0;

    public void MakeTradeOffer(string offererId, int lumber, int wool, int grain, int brick, int ore)
    {
        this.offererId = offererId;
        this.offererLumber = lumber;
        this.offererWool = wool;
        this.offererGrain = grain;
        this.offererBrick = brick;
        this.offererOre = ore;
    }

    /**
     * Accept the trade as a receiving player.
     */
    public void AcceptAsReceiver()
    {
        this.receiverAccept = true;
    }

    /**
     * Unaccept the trade as a receiving player.
     */
    public void UnAcceptAsReceiver()
    {
        this.receiverAccept = false;
    }

    /**
     * Accept the trade as an offering player.
     */
    public void AcceptAsOfferer()
    {
        this.offererAccept = true;
    }
    
    /**
     * Unaccept the trade as an offering player.
     */
    public void UnAcceptAsOfferer()
    {
        this.offererAccept = false;
    }

    /**
     * Resets confirmation for trade for all players.
     */
    public void UnAcceptAll()
    {
        this.receiverAccept = false;
        this.offererAccept = false;

        this.receiverWantsToTrade = false;
        this.offererWantsToTrade = false;
    }

    public void ResetTrade()
    {
        this.trading = false;

        this.receiverAccept = false;
        this.offererAccept = false;

        this.receiverWantsToTrade = false;
        this.offererWantsToTrade = false;

        this.receiverId = "";
        this.receiverBrick = 0;
        this.receiverGrain = 0;
        this.receiverLumber = 0;
        this.receiverOre = 0;
        this.receiverWool = 0;

        this.offererId = "";
        this.offererBrick = 0;
        this.offererGrain = 0;
        this.offererLumber = 0;
        this.offererOre = 0;
        this.offererWool = 0;
    }

    public bool IsPlayerOffering(string id)
    {
        return offererId.Equals(id) && trading;
    }

    public bool isPlayerReceiving(string id)
    {
        return receiverId.Equals(id) && trading;
    }

    public bool ShouldTradeResolve()
    {
        return receiverAccept && offererAccept && receiverWantsToTrade && offererWantsToTrade;
    }
}
