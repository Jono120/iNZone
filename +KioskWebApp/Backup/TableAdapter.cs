using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Synchronization.Data;
using Microsoft.Synchronization.Data.Server;
using System.Data.Common;
using System.Linq;

namespace InzoneKioskWebservice
{
    public class TableAdapter
    {

        public static SyncAdapter ParticipantSyncAdapter()
        {
        
            // Participant Table
            SyncAdapter adapterParticipant = new SyncAdapter("Participant");

            // select incremental inserts command
            SqlCommand incInsParticipantCmd = new SqlCommand();
            incInsParticipantCmd.CommandType = CommandType.Text;
            incInsParticipantCmd.CommandText = "SELECT ID, FirstName, LastName, Email, DateOfBirth, SecurityQuestion, SecurityAnswer, UserName, [Password], Address1, Address2, Suburb, Town, PhoneNumber, KnowsCareer, Gender, KioskID, DateCreated from [Participant] " +
                                   "where create_timestamp > @sync_last_received_anchor " +
                                     "and create_timestamp <= @sync_new_received_anchor " +
                                     "and update_originator_id <> @sync_client_id_hash " +
                                   "order by create_timestamp desc ";
            incInsParticipantCmd.Parameters.Add("@" + SyncSession.SyncClientIdHash, SqlDbType.Int);
            incInsParticipantCmd.Parameters.Add("@" + SyncSession.SyncLastReceivedAnchor, SqlDbType.Binary, 8);
            incInsParticipantCmd.Parameters.Add("@" + SyncSession.SyncNewReceivedAnchor, SqlDbType.Binary, 8);

            adapterParticipant.SelectIncrementalInsertsCommand = incInsParticipantCmd;
            
            // select incremental updates command
            SqlCommand incUpdParticipantCmd = incInsParticipantCmd.Clone();
            incUpdParticipantCmd.CommandText = "SELECT ID, FirstName, LastName, Email, DateOfBirth, SecurityQuestion, SecurityAnswer, UserName, [Password], Address1, Address2, Suburb, Town, PhoneNumber, KnowsCareer, Gender, KioskID, DateCreated from [Participant] " +
                                          "where create_timestamp <= @sync_last_received_anchor " +
                                            "and update_timestamp > @sync_last_received_anchor " +
                                            "and update_timestamp <= @sync_new_received_anchor	" +
                                            "and update_originator_id <> @sync_client_id_hash " +
                                          "order by update_timestamp desc ";

            adapterParticipant.SelectIncrementalUpdatesCommand = incUpdParticipantCmd;

            // select incremental deletes command
            SqlCommand incDelParticipantCmd = incInsParticipantCmd.Clone();
            incDelParticipantCmd.CommandText = "select ID from [Participant_tombstone] " +
                                          "where update_timestamp > @sync_last_received_anchor " +
                                            "and update_timestamp <= @sync_new_received_anchor " +
                                            "and update_originator_id <> @sync_client_id_hash " +
                                            "order by update_timestamp desc	";

            adapterParticipant.SelectIncrementalDeletesCommand = incDelParticipantCmd;


            // insert row com
            SqlCommand insParticipantCmd = new SqlCommand();
            insParticipantCmd.CommandType = CommandType.StoredProcedure;
            insParticipantCmd.CommandText = "usp_Participant_applyinsert";
            
            insParticipantCmd.Parameters.Add("@ID",SqlDbType.UniqueIdentifier);
            insParticipantCmd.Parameters.Add("@FirstName",SqlDbType.VarChar,30);
            insParticipantCmd.Parameters.Add("@LastName",SqlDbType.VarChar,30);
            insParticipantCmd.Parameters.Add("@Email",SqlDbType.VarChar,50);
            insParticipantCmd.Parameters.Add("@DateOfBirth", SqlDbType.DateTime);
            insParticipantCmd.Parameters.Add("@SecurityQuestion",SqlDbType.VarChar,50);
            insParticipantCmd.Parameters.Add("@SecurityAnswer",SqlDbType.VarChar,30);
            insParticipantCmd.Parameters.Add("@UserName",SqlDbType.VarChar,30);
            insParticipantCmd.Parameters.Add("@Password",SqlDbType.VarChar,20);
            insParticipantCmd.Parameters.Add("@Address1",SqlDbType.VarChar,50);
            insParticipantCmd.Parameters.Add("@Address2",SqlDbType.VarChar,50);
            insParticipantCmd.Parameters.Add("@Suburb",SqlDbType.VarChar,50);
            insParticipantCmd.Parameters.Add("@Town",SqlDbType.VarChar,30);
            insParticipantCmd.Parameters.Add("@PhoneNumber",SqlDbType.VarChar,30);
            insParticipantCmd.Parameters.Add("@KnowsCareer",SqlDbType.Bit);
            insParticipantCmd.Parameters.Add("@Gender",SqlDbType.Char,1);
            insParticipantCmd.Parameters.Add("@KioskID",SqlDbType.VarChar,20);
            insParticipantCmd.Parameters.Add("@DateCreated",SqlDbType.SmallDateTime);

            insParticipantCmd.Parameters.Add("@" + SyncSession.SyncClientIdHash, SqlDbType.Int);
            insParticipantCmd.Parameters.Add("@" + SyncSession.SyncLastReceivedAnchor, SqlDbType.Binary, 8);
            insParticipantCmd.Parameters.Add("@" + SyncSession.SyncRowCount, SqlDbType.Int).Direction = ParameterDirection.Output;

            adapterParticipant.InsertCommand = insParticipantCmd;


            // update row command
            SqlCommand updParticipantCmd = new SqlCommand();
            updParticipantCmd.CommandType = CommandType.StoredProcedure;
            updParticipantCmd.CommandText = "usp_Participant_applyupdate";

            updParticipantCmd.Parameters.Add("@ID",SqlDbType.UniqueIdentifier);
            updParticipantCmd.Parameters.Add("@FirstName",SqlDbType.VarChar,30);
            updParticipantCmd.Parameters.Add("@LastName",SqlDbType.VarChar,30);
            updParticipantCmd.Parameters.Add("@Email",SqlDbType.VarChar,50);
            updParticipantCmd.Parameters.Add("@DateOfBirth", SqlDbType.DateTime);
            updParticipantCmd.Parameters.Add("@SecurityQuestion",SqlDbType.VarChar,50);
            updParticipantCmd.Parameters.Add("@SecurityAnswer",SqlDbType.VarChar,30);
            updParticipantCmd.Parameters.Add("@UserName",SqlDbType.VarChar,30);
            updParticipantCmd.Parameters.Add("@Password",SqlDbType.VarChar,20);
            updParticipantCmd.Parameters.Add("@Address1",SqlDbType.VarChar,50);
            updParticipantCmd.Parameters.Add("@Address2",SqlDbType.VarChar,50);
            updParticipantCmd.Parameters.Add("@Suburb",SqlDbType.VarChar,50);
            updParticipantCmd.Parameters.Add("@Town",SqlDbType.VarChar,30);
            updParticipantCmd.Parameters.Add("@PhoneNumber",SqlDbType.VarChar,30);
            updParticipantCmd.Parameters.Add("@KnowsCareer",SqlDbType.Bit);
            updParticipantCmd.Parameters.Add("@Gender",SqlDbType.Char,1);
            updParticipantCmd.Parameters.Add("@KioskID",SqlDbType.VarChar,20);
            updParticipantCmd.Parameters.Add("@DateCreated",SqlDbType.SmallDateTime);
            
            updParticipantCmd.Parameters.Add("@" + SyncSession.SyncClientIdHash, SqlDbType.Int);
            updParticipantCmd.Parameters.Add("@" + SyncSession.SyncLastReceivedAnchor, SqlDbType.Binary, 8);
            updParticipantCmd.Parameters.Add("@" + SyncSession.SyncRowCount, SqlDbType.Int).Direction = ParameterDirection.Output;

            adapterParticipant.UpdateCommand = updParticipantCmd;


            // delete row command
            SqlCommand delParticipantCmd = new SqlCommand();
            delParticipantCmd.CommandType = CommandType.StoredProcedure;

            delParticipantCmd.CommandText = "usp_Participant_applydelete";
            delParticipantCmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
            
            delParticipantCmd.Parameters.Add("@" + SyncSession.SyncClientIdHash, SqlDbType.Int);
            delParticipantCmd.Parameters.Add("@" + SyncSession.SyncLastReceivedAnchor, SqlDbType.Binary, 8);
            delParticipantCmd.Parameters.Add("@" + SyncSession.SyncRowCount, SqlDbType.Int).Direction = ParameterDirection.Output;

            adapterParticipant.DeleteCommand = delParticipantCmd;

            // get update conflicting row command
            SqlCommand updcftParticipantCmd = new SqlCommand();
            updcftParticipantCmd.CommandType = CommandType.StoredProcedure;
            updcftParticipantCmd.CommandText = "usp_Participant_getupdateconflict";
            updcftParticipantCmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);

            adapterParticipant.SelectConflictUpdatedRowsCommand = updcftParticipantCmd;

            // get delete conflicting row command
            SqlCommand delcftParticipantCmd = new SqlCommand();
            delcftParticipantCmd.CommandType = CommandType.StoredProcedure;
            delcftParticipantCmd.CommandText = "usp_Participant_getdeleteconflict";
            delcftParticipantCmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);

            adapterParticipant.SelectConflictDeletedRowsCommand = delcftParticipantCmd;

            return adapterParticipant;
        }

        public static SyncAdapter InteractionSyncAdapter()
        {
            // Interaction Table
            SyncAdapter adapterInteraction = new SyncAdapter("Interaction");

            // select incremental inserts command
            SqlCommand incInsInteractionCmd = new SqlCommand();
            incInsInteractionCmd.CommandType = CommandType.Text;
            incInsInteractionCmd.CommandText = "SELECT ID, ParticipantID, PartnerID, Subscribed, DateCreated from [Interaction] " +
                                   "where create_timestamp > @sync_last_received_anchor " +
                                     "and create_timestamp <= @sync_new_received_anchor " +
                                     "and update_originator_id <> @sync_client_id_hash " +
                                   "order by create_timestamp desc ";
            incInsInteractionCmd.Parameters.Add("@" + SyncSession.SyncClientIdHash, SqlDbType.Int);
            incInsInteractionCmd.Parameters.Add("@" + SyncSession.SyncLastReceivedAnchor, SqlDbType.Binary, 8);
            incInsInteractionCmd.Parameters.Add("@" + SyncSession.SyncNewReceivedAnchor, SqlDbType.Binary, 8);

            adapterInteraction.SelectIncrementalInsertsCommand = incInsInteractionCmd;

            // select incremental updates command
            SqlCommand incUpdInteractionCmd = incInsInteractionCmd.Clone();
            incUpdInteractionCmd.CommandText = "SELECT ID, ParticipantID, PartnerID, Subscribed, DateCreated from [Interaction] " +
                                          "where create_timestamp <= @sync_last_received_anchor " +
                                            "and update_timestamp > @sync_last_received_anchor " +
                                            "and update_timestamp <= @sync_new_received_anchor	" +
                                            "and update_originator_id <> @sync_client_id_hash " +
                                          "order by update_timestamp desc ";

            adapterInteraction.SelectIncrementalUpdatesCommand = incUpdInteractionCmd;

            // select incremental deletes command
            SqlCommand incDelInteractionCmd = incInsInteractionCmd.Clone();
            incDelInteractionCmd.CommandText = "select ID from [Interaction_tombstone] " +
                                          "where update_timestamp > @sync_last_received_anchor " +
                                            "and update_timestamp <= @sync_new_received_anchor " +
                                            "and update_originator_id <> @sync_client_id_hash " +
                                            "order by update_timestamp desc	";

            adapterInteraction.SelectIncrementalDeletesCommand = incDelInteractionCmd;


            // insert row com
            SqlCommand insInteractionCmd = new SqlCommand();
            insInteractionCmd.CommandType = CommandType.StoredProcedure;
            insInteractionCmd.CommandText = "usp_Interaction_applyinsert";

            insInteractionCmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
            insInteractionCmd.Parameters.Add("@ParticipantID", SqlDbType.UniqueIdentifier);
            insInteractionCmd.Parameters.Add("@PartnerID", SqlDbType.Int);
            insInteractionCmd.Parameters.Add("@Subscribed", SqlDbType.Int);
            insInteractionCmd.Parameters.Add("@DateCreated", SqlDbType.SmallDateTime);

            insInteractionCmd.Parameters.Add("@" + SyncSession.SyncClientIdHash, SqlDbType.Int);
            insInteractionCmd.Parameters.Add("@" + SyncSession.SyncLastReceivedAnchor, SqlDbType.Binary, 8);
            insInteractionCmd.Parameters.Add("@" + SyncSession.SyncRowCount, SqlDbType.Int).Direction = ParameterDirection.Output;

            adapterInteraction.InsertCommand = insInteractionCmd;


            // update row command
            SqlCommand updInteractionCmd = new SqlCommand();
            updInteractionCmd.CommandType = CommandType.StoredProcedure;
            updInteractionCmd.CommandText = "usp_Interaction_applyupdate";

            updInteractionCmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
            updInteractionCmd.Parameters.Add("@ParticipantID", SqlDbType.UniqueIdentifier);
            updInteractionCmd.Parameters.Add("@PartnerID", SqlDbType.Int);
            updInteractionCmd.Parameters.Add("@Subscribed", SqlDbType.Int);
            updInteractionCmd.Parameters.Add("@DateCreated", SqlDbType.SmallDateTime);

            updInteractionCmd.Parameters.Add("@" + SyncSession.SyncClientIdHash, SqlDbType.Int);
            updInteractionCmd.Parameters.Add("@" + SyncSession.SyncLastReceivedAnchor, SqlDbType.Binary, 8);
            updInteractionCmd.Parameters.Add("@" + SyncSession.SyncRowCount, SqlDbType.Int).Direction = ParameterDirection.Output;

            adapterInteraction.UpdateCommand = updInteractionCmd;


            // delete row command
            SqlCommand delInteractionCmd = new SqlCommand();
            delInteractionCmd.CommandType = CommandType.StoredProcedure;

            delInteractionCmd.CommandText = "usp_Interaction_applydelete";
            delInteractionCmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);

            delInteractionCmd.Parameters.Add("@" + SyncSession.SyncClientIdHash, SqlDbType.Int);
            delInteractionCmd.Parameters.Add("@" + SyncSession.SyncLastReceivedAnchor, SqlDbType.Binary, 8);
            delInteractionCmd.Parameters.Add("@" + SyncSession.SyncRowCount, SqlDbType.Int).Direction = ParameterDirection.Output;

            adapterInteraction.DeleteCommand = delInteractionCmd;


            // get update conflicting row command
            SqlCommand updcftInteractionCmd = new SqlCommand();
            updcftInteractionCmd.CommandType = CommandType.StoredProcedure;
            updcftInteractionCmd.CommandText = "usp_Interaction_getupdateconflict";
            updcftInteractionCmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);

            adapterInteraction.SelectConflictUpdatedRowsCommand = updcftInteractionCmd;


            // get delete conflicting row command
            SqlCommand delcftInteractionCmd = new SqlCommand();
            delcftInteractionCmd.CommandType = CommandType.StoredProcedure;
            delcftInteractionCmd.CommandText = "usp_Interaction_getdeleteconflict";
            delcftInteractionCmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);

            adapterInteraction.SelectConflictDeletedRowsCommand = delcftInteractionCmd;


            return adapterInteraction;
        }

        public static SyncAdapter InteractionVideoSyncAdapter()
        {
            // InteractionVideo Table
            SyncAdapter adapterInteractionVideo = new SyncAdapter("InteractionVideo");


            // select incremental inserts command
            SqlCommand incInsInteractionVideoCmd = new SqlCommand();
            incInsInteractionVideoCmd.CommandType = CommandType.Text;
            incInsInteractionVideoCmd.CommandText = "SELECT ID, InteractionID, VideoRating, VideoID, VideoName, DateCreated FROM [InteractionVideo] " +
                                   "where create_timestamp > @sync_last_received_anchor " +
                                     "and create_timestamp <= @sync_new_received_anchor " +
                                     "and update_originator_id <> @sync_client_id_hash " +
                                   "order by create_timestamp desc ";
            incInsInteractionVideoCmd.Parameters.Add("@" + SyncSession.SyncClientIdHash, SqlDbType.Int);
            incInsInteractionVideoCmd.Parameters.Add("@" + SyncSession.SyncLastReceivedAnchor, SqlDbType.Binary, 8);
            incInsInteractionVideoCmd.Parameters.Add("@" + SyncSession.SyncNewReceivedAnchor, SqlDbType.Binary, 8);

            adapterInteractionVideo.SelectIncrementalInsertsCommand = incInsInteractionVideoCmd;


            // select incremental updates command
            SqlCommand incUpdInteractionVideoCmd = incInsInteractionVideoCmd.Clone();
            incUpdInteractionVideoCmd.CommandText = "SELECT ID, InteractionID, VideoRating, VideoID, VideoName, DateCreated FROM [InteractionVideo] " +
                                          "where create_timestamp <= @sync_last_received_anchor " +
                                            "and update_timestamp > @sync_last_received_anchor " +
                                            "and update_timestamp <= @sync_new_received_anchor	" +
                                            "and update_originator_id <> @sync_client_id_hash " +
                                          "order by update_timestamp desc ";

            adapterInteractionVideo.SelectIncrementalUpdatesCommand = incUpdInteractionVideoCmd;

            // select incremental deletes command
            SqlCommand incDelInteractionVideoCmd = incInsInteractionVideoCmd.Clone();
            incDelInteractionVideoCmd.CommandText = "select ID from [InteractionVideo_tombstone] " +
                                          "where update_timestamp > @sync_last_received_anchor " +
                                            "and update_timestamp <= @sync_new_received_anchor " +
                                            "and update_originator_id <> @sync_client_id_hash " +
                                            "order by update_timestamp desc	";

            adapterInteractionVideo.SelectIncrementalDeletesCommand = incDelInteractionVideoCmd;

            // insert row com
            SqlCommand insInteractionVideoCmd = new SqlCommand();
            insInteractionVideoCmd.CommandType = CommandType.StoredProcedure;
            insInteractionVideoCmd.CommandText = "usp_InteractionVideo_applyinsert";

            insInteractionVideoCmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
            insInteractionVideoCmd.Parameters.Add("@InteractionID", SqlDbType.UniqueIdentifier);
            insInteractionVideoCmd.Parameters.Add("@VideoRating", SqlDbType.TinyInt);
            insInteractionVideoCmd.Parameters.Add("@VideoID", SqlDbType.TinyInt);
            insInteractionVideoCmd.Parameters.Add("@VideoName", SqlDbType.VarChar, 30);
            insInteractionVideoCmd.Parameters.Add("@DateCreated", SqlDbType.SmallDateTime);

            insInteractionVideoCmd.Parameters.Add("@" + SyncSession.SyncClientIdHash, SqlDbType.Int);
            insInteractionVideoCmd.Parameters.Add("@" + SyncSession.SyncLastReceivedAnchor, SqlDbType.Binary, 8);
            insInteractionVideoCmd.Parameters.Add("@" + SyncSession.SyncRowCount, SqlDbType.Int).Direction = ParameterDirection.Output;

            adapterInteractionVideo.InsertCommand = insInteractionVideoCmd;


            // update row command
            SqlCommand updInteractionVideoCmd = new SqlCommand();
            updInteractionVideoCmd.CommandType = CommandType.StoredProcedure;
            updInteractionVideoCmd.CommandText = "usp_InteractionVideo_applyupdate";

            updInteractionVideoCmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
            updInteractionVideoCmd.Parameters.Add("@InteractionID", SqlDbType.UniqueIdentifier);
            updInteractionVideoCmd.Parameters.Add("@VideoRating", SqlDbType.TinyInt);
            updInteractionVideoCmd.Parameters.Add("@VideoID", SqlDbType.TinyInt);
            updInteractionVideoCmd.Parameters.Add("@VideoName", SqlDbType.VarChar, 30);
            updInteractionVideoCmd.Parameters.Add("@DateCreated", SqlDbType.SmallDateTime);

            updInteractionVideoCmd.Parameters.Add("@" + SyncSession.SyncClientIdHash, SqlDbType.Int);
            updInteractionVideoCmd.Parameters.Add("@" + SyncSession.SyncLastReceivedAnchor, SqlDbType.Binary, 8);
            updInteractionVideoCmd.Parameters.Add("@" + SyncSession.SyncRowCount, SqlDbType.Int).Direction = ParameterDirection.Output;

            adapterInteractionVideo.UpdateCommand = updInteractionVideoCmd;


            // delete row command
            SqlCommand delInteractionVideoCmd = new SqlCommand();
            delInteractionVideoCmd.CommandType = CommandType.StoredProcedure;

            delInteractionVideoCmd.CommandText = "usp_InteractionVideo_applydelete";
            delInteractionVideoCmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);

            delInteractionVideoCmd.Parameters.Add("@" + SyncSession.SyncClientIdHash, SqlDbType.Int);
            delInteractionVideoCmd.Parameters.Add("@" + SyncSession.SyncLastReceivedAnchor, SqlDbType.Binary, 8);
            delInteractionVideoCmd.Parameters.Add("@" + SyncSession.SyncRowCount, SqlDbType.Int).Direction = ParameterDirection.Output;

            adapterInteractionVideo.DeleteCommand = delInteractionVideoCmd;


            // get update conflicting row command
            SqlCommand updcftInteractionVideoCmd = new SqlCommand();
            updcftInteractionVideoCmd.CommandType = CommandType.StoredProcedure;
            updcftInteractionVideoCmd.CommandText = "usp_InteractionVideo_getupdateconflict";
            updcftInteractionVideoCmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);

            adapterInteractionVideo.SelectConflictUpdatedRowsCommand = updcftInteractionVideoCmd;

            // get delete conflicting row command
            SqlCommand delcftInteractionVideoCmd = new SqlCommand();
            delcftInteractionVideoCmd.CommandType = CommandType.StoredProcedure;
            delcftInteractionVideoCmd.CommandText = "usp_InteractionVideo_getdeleteconflict";
            delcftInteractionVideoCmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);

            adapterInteractionVideo.SelectConflictDeletedRowsCommand = delcftInteractionVideoCmd;

            return adapterInteractionVideo;
        }
    }
}
