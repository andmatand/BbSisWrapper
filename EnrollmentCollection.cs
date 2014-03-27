using Blackbaud.PIA.EA7.BBEEAPI7;
using System;
using System.Collections.ObjectModel;

namespace BbSisWrapper {
    public partial class Student {
        public class EnrollmentCollection : Collection<Enrollment> {
            private cEAAppsEnrolls bbCollection;

            public EnrollmentCollection(cEAAppsEnrolls bbCollection) {
                this.bbCollection = bbCollection;

                foreach (cEAAppEnroll bbRecord in bbCollection) {
                    Add(new Student.Enrollment(bbRecord));
                }
            }

            public Student.Enrollment Add() {
                var newEnrollment = new Student.Enrollment(bbCollection.Add());
                Add(newEnrollment);

                return newEnrollment;
            }
        }
    }
}