﻿using Blackbaud.PIA.EA7.BBEEAPI7;
using System;
using System.Collections.Generic;
using FIELD = Blackbaud.PIA.EA7.BBEEAPI7.EEAINDIVIDUALSFields;
using IBBAddressHeaders = Blackbaud.PIA.FE7.AFNInterfaces.IBBAddressHeaders;

namespace BbSisWrapper {
    public class Individual : IPerson {
        private CEAIndividualRecord bbRecord;
        private IBBSessionContext context;
        private AddressCollection addresses;

        public Individual(CEAIndividualRecord bbRecord, IBBSessionContext context) {
            this.bbRecord = bbRecord;
            this.context = context;
        }

        ~Individual() {
            Close();
        }

        public bool CanBeSaved {
            get { return bbRecord.CanBeSaved(); }
        }

        public ReasonRecordCannotBeSaved ReasonRecordCannotBeSaved {
            get {
                bbCantSaveReasons bbCantSaveReason = bbCantSaveReasons.csrObjectVersionOutOfDate;
                string message = null;

                if (!bbRecord.CanBeSaved(ref bbCantSaveReason, ref message)) {
                    return new ReasonRecordCannotBeSaved(bbCantSaveReason, message);
                }
                else {
                    return null;
                }
            }
        }

        public DateTime DateAdded {
            get {
                return DateTime.Parse((string) bbRecord.Fields[FIELD.EAINDIVIDUALS_fld_DATEADDED]);
            }
        }

        public int Ea7IndividualsId {
            get {
                return int.Parse((string)
                    bbRecord.Fields[FIELD.EAINDIVIDUALS_fld_EA7INDIVIDUALSID]);
            }
        }

        public int Ea7RecordsId {
            get {
                return int.Parse((string) bbRecord.Fields[FIELD.EAINDIVIDUALS_fld_EA7RECORDSID]);
            }
        }

        public string IdNumber {
            get {
                return (string) bbRecord.Fields[FIELD.EAINDIVIDUALS_fld_USERDEFINEDID];
            }
        }

        public string FirstName {
            get {
                return (string) bbRecord.Fields[FIELD.EAINDIVIDUALS_fld_FIRSTNAME];
            }
            set {
                bbRecord.Fields[FIELD.EAINDIVIDUALS_fld_FIRSTNAME] = value;
            }
        }

        public PersonGender Gender {
            get {
                switch ((string) bbRecord.Fields[FIELD.EAINDIVIDUALS_fld_GENDER]) {
                    case "Male":
                        return PersonGender.Male;
                    case "Female":
                        return PersonGender.Female;
                    default:
                        return PersonGender.Null;
                }
            }
        }

        public string Nickname {
            get {
                return (string) bbRecord.Fields[FIELD.EAINDIVIDUALS_fld_NICKNAME];
            }
            set {
                bbRecord.Fields[FIELD.EAINDIVIDUALS_fld_NICKNAME] = value;
            }
        }

        public string MiddleName {
            get {
                return (string) bbRecord.Fields[FIELD.EAINDIVIDUALS_fld_MIDDLENAME];
            }
            set {
                bbRecord.Fields[FIELD.EAINDIVIDUALS_fld_MIDDLENAME] = value;
            }
        }

        public string LastName {
            get {
                return (string) bbRecord.Fields[FIELD.EAINDIVIDUALS_fld_LASTNAME];
            }
            set {
                bbRecord.Fields[FIELD.EAINDIVIDUALS_fld_LASTNAME] = value;
            }
        }

        public string OnlineUserId {
            get {
                return (string) bbRecord.Fields[FIELD.EAINDIVIDUALS_fld_ONLINEUSERID];
            }
            set {
                bbRecord.Fields[FIELD.EAINDIVIDUALS_fld_ONLINEUSERID] = value;
            }
        }

        public string OnlinePassword {
            get {
                return (string) bbRecord.Fields[FIELD.EAINDIVIDUALS_fld_ONLINEPASSWORD];
            }
            set {
                bbRecord.Fields[FIELD.EAINDIVIDUALS_fld_ONLINEPASSWORD] = value;
            }
        }

        public AddressCollection Addresses {
            get {
                if (addresses == null) {
                    addresses = new AddressCollection((IBBAddressHeaders) bbRecord.Address);
                }

                return addresses;
            }
        }

        public void Close() {
            if (bbRecord == null) return;

            // If we have addresses loaded
            if (addresses != null) {
                // Clear our list of addresses
                addresses = null;
            }

            // Release our handle on the SIS record
            bbRecord.CloseDown();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(bbRecord);
            bbRecord = null;
        }

        public void Reload() {
            // Save our Ea7IndividualsId
            int ea7IndividualsID = Ea7IndividualsId;

            // Close the SIS record
            Close();

            // Load the same individual again
            bbRecord = LoadBbRecord(ea7IndividualsID, context);
        }

        public void Save() {
            bbRecord.Save();
        }

        private static CEAIndividualRecord LoadBbRecord(int ea7IndividualsID,
                                                        IBBSessionContext context) {
            var record = new CEAIndividualRecord();
            record.Init(context);
            record.Load(ea7IndividualsID);

            return record;
        }

        public static Individual LoadByEa7IndividualsId(int ea7IndividualsID,
                                                        IBBSessionContext context) {
            var bbRecord = LoadBbRecord(ea7IndividualsID, context);
            return new Individual(bbRecord, context);
        }

        public static IEnumerable<Individual>
        LoadCollection(
            Context context,
            string sqlFrom = null,
            string sqlWhere = null,
            string sqlOrderBy = null)
        {
            return LoadCollection(context.BbSisContext, sqlFrom, sqlWhere, sqlOrderBy);
        }

        internal static IEnumerable<Individual>
        LoadCollection(
            IBBSessionContext context,
            string sqlFrom = null,
            string sqlWhere = null,
            string sqlOrderBy = null)
        {
            CEAIndividualRecords bbCollection = new CEAIndividualRecords();
            bbCollection.Init(context);

            if (sqlFrom != null) {
                bbCollection.FilterObject.CustomFilterProperty[
                    eDataFilterCustomTypes.CUSTOMFILTERTYPE_CUSTOMFROM] = sqlFrom;
            }
            if (sqlWhere != null) {
                bbCollection.FilterObject.CustomFilterProperty[
                    eDataFilterCustomTypes.CUSTOMFILTERTYPE_CUSTOMWHERE] = sqlWhere;
            }
            if (sqlOrderBy != null) {
                bbCollection.FilterObject.CustomFilterProperty[
                    eDataFilterCustomTypes.CUSTOMFILTERTYPE_CUSTOMORDERBY] = sqlOrderBy;
            }

            foreach (CEAIndividualRecord bbObject in bbCollection) {
                yield return new Individual(bbObject, context);
            }

            bbCollection.CloseDown();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(bbCollection);
            bbCollection = null;
        }


        public static Individual Create(IBBSessionContext context) {
            CEAIndividualRecord bbRecord = new CEAIndividualRecord();
            bbRecord.Init(context);

            return new Individual(bbRecord, context);
        }
    }
}