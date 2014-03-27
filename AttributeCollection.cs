using Blackbaud.PIA.FE7.AFNInterfaces;
using System.Collections.ObjectModel;

namespace BbSisWrapper {
    public class AttributeCollection : Collection<Attribute> {
        private IBBAttributesAPI bbCollection;

        public AttributeCollection(IBBAttributesAPI bbCollection) {
            this.bbCollection = bbCollection;

            foreach (IBBAttribute bbRecord in bbCollection) {
                Items.Add(new Attribute(bbRecord));
            }
        }

        public new void Remove(Attribute item) {
            // Remove the BB item from the BB collection
            bbCollection.Remove(item.SisObject);

            // Remove the wrapper item from the wrapper collection
            Items.Remove(item);
        }

        public new void RemoveAt(int index) {
            Remove(Items[index]);
        }
    }
}