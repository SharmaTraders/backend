﻿namespace UnitTests.Factory;

public static class BillingPartyFactory {
    public static IEnumerable<object[]> GetValidBillingPartyNames() {
        return new List<object[]>() {
            new object[] {"3ch"},
            new object[] {"Sachin Private Limited"},
            new object[] {"a".PadRight(30, 'a')},
            new object[] {"a".PadRight(29, 'a')},
        };
    }

    public static IEnumerable<object[]> GetInValidBillingPartyNames() {
        return new List<object[]>() {
            new object[] {null},
            new object[] {""},
            new object[] {"2c"},
            new object[] {"a"},
            new object[] {"a".PadRight(31, 'a')},
            new object[] {"a".PadRight(50, 'a')},
        };
    }

    public static IEnumerable<object[]> GetValidBillingPartyAddress() {
        return new List<object[]>() {
            new object[] {"3ch"},
            new object[] {"Pokhara, Nepal 8200"},
            new object[] {"a".PadRight(60, 'a')},
            new object[] {"a".PadRight(59, 'a')},
        };
    }

    public static IEnumerable<object[]> GetInValidBillingPartyAddress() {
        return new List<object[]>() {
            new object[] {null},
            new object[] {""},
            new object[] {"2c"},
            new object[] {"a"},
            new object[] {"a".PadRight(61, 'a')},
            new object[] {"a".PadRight(100, 'a')},
        };
    }

    public static IEnumerable<object[]> GetValidBillingPartyEmails() {
        return new List<object[]>() {
            // Optional so can be empty and null
            new object[] {null},
            new object[] {""},
            new object[] {"310628@via.dk"},
            new object[] {"ALHE@via.dk"},
            new object[] {"TRMO@via.dk"},
            new object[] {"sachinbaral@hotmail.dk"},
            new object[] {"himalSharma123@yahoo.dk"}
        };
    }

    public static IEnumerable<object[]> GetValidBillingPartyEmailsNotEmpty() {
        return new List<object[]>() {
            // Optional so can be empty and null
            new object[] {"310628@via.dk"},
            new object[] {"ALHE@via.dk"},
            new object[] {"TRMO@via.dk"},
            new object[] {"sachinbaral@hotmail.dk"},
            new object[] {"himalSharma123@yahoo.dk"}
        };
    }

    public static IEnumerable<object[]> GetInValidBillingPartyEmails() {
        return new List<object[]>() {
            new object[] {"@via.dk"},
            new object[] {"Hello"},
            new object[] {"Hello@"},
            new object[] {"Hello@gmail"},
            new object[] {"Hello.com"},
        };
    } 

    public static IEnumerable<object[]> GetValidBillingPartyPhoneNumber() {
        return new List<object[]>() {
            new object[] {null},
            new object[] {""},
            new object[] {"0123456789"},
            new object[] {"9846038050"},
            new object[] {"0000000000"},
        };
    } 

    public static IEnumerable<object[]> GetInValidBillingPartyPhoneNumber() {
        return new List<object[]>() {
            new object[] {"1"},
            new object[] {"12"},
            new object[] {"123456789"},
            new object[] {"98460380502500"},
            new object[] {"123456789101112131"},
            new object[] {"thisis10ch"},
            new object[] {"a".PadRight(10, 'a')},
        };
    }
}