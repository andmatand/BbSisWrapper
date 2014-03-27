using Blackbaud.PIA.FE7.AFNInterfaces;
using System;
using System.Collections.ObjectModel;

namespace BbSisWrapper {
    public class AddressCollection : Collection<Address> {
        private IBBAddressHeaders bbCollection;

        public AddressCollection(IBBAddressHeaders bbCollection) {
            this.bbCollection = bbCollection;

            // Load each of the address records
            foreach (IBBAddressHeader bbRecord in bbCollection) {
                Items.Add(new Address(bbRecord));
            }
        }

        public Address Add() {
            IBBAddressHeader bbAddress = bbCollection.Add();
            var newAddress = new Address(bbAddress);
            Items.Add(newAddress);

            return newAddress;
        }
    }
}