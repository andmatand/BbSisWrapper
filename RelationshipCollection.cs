﻿using Blackbaud.PIA.EA7.BBEEAPI7;
using System;
using System.Collections.ObjectModel;
using FIELDS = Blackbaud.PIA.EA7.BBEEAPI7.EEARELATIONSHIPSFields;

namespace BbSisWrapper {
    public class RelationshipCollection : Collection<IRelationship> {
        private CEARelationShips bbCollection;

        public RelationshipCollection(CEARelationShips bbCollection) {
            this.bbCollection = bbCollection;

            // Load each of the address records
            foreach (CEARelationShip bbRecord in bbCollection) {
                EEARelationTypes relationshipType = (EEARelationTypes)
                    Enum.Parse(typeof(EEARelationTypes),
                        (string) bbRecord.Fields[FIELDS.EARELATIONSHIPS_fld_RELATIONIDTYPE]);

                switch (relationshipType) {
                    case EEARelationTypes.EARelationType_Person:
                        Add(new PersonalRelationship(bbRecord));
                        break;
                    case EEARelationTypes.EARelationType_Organization:
                        // TODO: implement OrganizationRelationship class
                        //Add(new OrganizationRelationship(bbRecord));
                        break;
                }
            }
        }

        public IRelationship Add(EEARelationTypes relationshipType) {
            switch (relationshipType) {
                case EEARelationTypes.EARelationType_Person:
                    // Add a new BB relationship object
                    CEARelationShip newBBRecord = bbCollection.Add();

                    // Set the type of the BB relationship object
                    newBBRecord.Fields[FIELDS.EARELATIONSHIPS_fld_RELATIONIDTYPE] =
                        relationshipType;

                    // Return a wrapper object for the BB relationship
                    return new PersonalRelationship(newBBRecord);
                case EEARelationTypes.EARelationType_Organization:
                    throw new NotImplementedException();
            }

            // Supress compiler warning
            return null;
        }
    }
}