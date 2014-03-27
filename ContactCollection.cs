using Blackbaud.PIA.FE7.AFNInterfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace BbSisWrapper {
    public class ContactCollection : Collection<Address.Contact> {
        private IBBPhonesAPI bbPhones;
        private IBBPhoneHeaders bbPhoneHeaders;

        public ContactCollection(IBBPhonesAPI bbPhones, IBBPhoneHeaders bbPhoneHeaders) {
            this.bbPhones = bbPhones;
            this.bbPhoneHeaders = bbPhoneHeaders;

            if (bbPhoneHeaders != null) {
                foreach (IBBPhoneHeader bbPhoneHeader in bbPhoneHeaders) {
                    var contact = new Address.Contact(bbPhoneHeader);
                    Add(contact);
                }
            }
            else {
                foreach (IBBPhone bbPhone in bbPhones) {
                    var contact = new Address.Contact(bbPhone);
                    Add(contact);
                }
            }
        }

        public bool IsShared {
            get {
                return bbPhoneHeaders.AddressLinked;
            }
        }

        public new void Remove(Address.Contact contact) {
            if (bbPhoneHeaders != null) {
                bbPhoneHeaders.Remove(contact.SharingComponent.SisObject);
            }
            else {
                bbPhones.Remove(contact.SisObject);
            }

            Items.Remove(contact);
        }

        public void Remove(string contactType) {
            Items.Remove(Items.Single(x => x.Type == contactType));
        }

        public Address.Contact Add() {
            if (bbPhoneHeaders != null) {
                IBBPhoneHeader newBBRecord = bbPhoneHeaders.Add();
                var newContact = new Address.Contact(newBBRecord);
                return newContact;
            }
            else {
                IBBPhone newBBRecord = bbPhones.Add();
                var newContact = new Address.Contact(newBBRecord);
                return newContact;
            }
        }

        public Address.Contact this[string contactType] {
            get {
                return Items.FirstOrDefault(x => x.Type == contactType);
            }
        }

        public bool Contains(string contactType) {
            return Items.FirstOrDefault(x => x.Type == contactType) != null;
        }

        public void SetOrAdd(string contactType, string contactNumber, bool canBeShared = true) {
            Address.Contact targetContact = null;

            // Look for an existing contact of this type
            targetContact = Items.FirstOrDefault(x => x.Type == contactType);

            // If there is no existing contact of this type
            if (targetContact == null) {
                // Add a new contact
                targetContact = Add();
                targetContact.Type = contactType;
            }

            // If we found a contact on which to operate
            if (targetContact != null) {
                if (targetContact.SharingComponent != null) {
                    targetContact.SharingComponent.CanBeShared = canBeShared;
                }

                targetContact.Number = contactNumber;
            }
        }
    }
}