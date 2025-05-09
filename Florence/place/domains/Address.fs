namespace Florence

type Address = {
    Street: string
    Number: string
    Prefix: string
}

type AddressHolder =
    | Standard of Address //prefix means street, square etc
    // BuildingName in Dubai or Makani system (10digit numbers)