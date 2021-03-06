﻿using ACE.Entity;

namespace ACE.Network.GameAction
{
    [GameAction(GameActionOpcode.AdvocateTeleport)]
    public class GameActionAdvocateTeleport : GameActionPacket
    {
        private string target;
        private Position position;

        public GameActionAdvocateTeleport(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Read()
        {
            target   = fragment.Payload.ReadString16L();
            position = new Position(fragment.Payload);
        }

        public override void Handle()
        {
            // this check is also done clientside, see: PlayerDesc::PlayerIsPSR
            if (!session.Character.PropertiesBool.ContainsKey(PropertyBool.IsAdmin) && !session.Character.PropertiesBool.ContainsKey(PropertyBool.IsArch) && !session.Character.PropertiesBool.ContainsKey(PropertyBool.IsPsr))
                return;

            uint cell  = position.Cell;
            uint cellX = (cell >> 3);

            //TODO: Wrap command in a check to confirm session.character IsAdvocate or higher access level
            
            //TODO: Maybe output to chat window coords teleported to.
            //ChatPacket.SendSystemMessage(session, $"Teleporting to: 0.0[N/S], 0.0[E/W]");
            ChatPacket.SendSystemMessage(session, "Teleporting...");
            session.Character.Teleport(position);
        }
    }
}