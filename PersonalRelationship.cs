using Blackbaud.PIA.EA7.BBEEAPI7;
using System;
using FIELDS = Blackbaud.PIA.EA7.BBEEAPI7.EEARELATIONSHIPSFields;

namespace BbSisWrapper {
    public class PersonalRelationship : IRelationship {
        private CEARelationShip bbRecord;

        public PersonalRelationship(CEARelationShip bbRecord) {
            EEARelationTypes relationshipType = (EEARelationTypes)
                Enum.Parse(typeof(EEARelationTypes),
                           (string) bbRecord.Fields[FIELDS.EARELATIONSHIPS_fld_RELATIONIDTYPE]);

            if (relationshipType == EEARelationTypes.EARelationType_Person) {
                this.bbRecord = bbRecord;
            }
            else {
                throw new Exception("bbRecord RELATIONIDTYPE must be Person, not Organization");
            }
        }

        public int ParentId {
            get {
                return int.Parse((string) bbRecord.Fields[FIELDS.EARELATIONSHIPS_fld_PARENTID]);
            }
        }

        public int RelationId {
            get {
                return int.Parse((string) bbRecord.Fields[FIELDS.EARELATIONSHIPS_fld_RELATIONID]);
            }
            set {
                bbRecord.Fields[FIELDS.EARELATIONSHIPS_fld_RELATIONID] = value;
            }
        }

        public bool ViewNetClassroom {
            get {
                return (bbTF) Enum.Parse(
                    typeof(bbTF),
                    (string) bbRecord.Fields[FIELDS.EARELATIONSHIPS_fld_ALLOWNETCLASSROOMVIEW]) ==
                    bbTF.bbTrue;
            }
            set {
                bbRecord.Fields[FIELDS.EARELATIONSHIPS_fld_ALLOWNETCLASSROOMVIEW] =
                    (value ? bbTF.bbTrue : bbTF.bbFalse);
            }
        }
    }
}