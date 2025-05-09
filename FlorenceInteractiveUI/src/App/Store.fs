namespace App

module Store =
    let famousFlorencePeople = """{
  "type": "FeatureCollection",
  "features": [
    {
      "type": "Feature",
      "properties": {
        "name": "Dante Alighieri",
        "description": "Casa di Dante – traditional birthplace and now a museum"
      },
      "geometry": {
        "type": "Point",
        "coordinates": [11.2588, 43.7734]
      }
    },
    {
      "type": "Feature",
      "properties": {
        "name": "Leonardo da Vinci",
        "description": "Plaque on Via dei Gondi marking his presence in Florence"
      },
      "geometry": {
        "type": "Point",
        "coordinates": [11.2550, 43.7688]
      }
    },
    {
      "type": "Feature",
      "properties": {
        "name": "Michelangelo Buonarroti",
        "description": "Casa Buonarroti – family home and now a museum"
      },
      "geometry": {
        "type": "Point",
        "coordinates": [11.2666, 43.7694]
      }
    },
    {
      "type": "Feature",
      "properties": {
        "name": "Galileo Galilei",
        "description": "Residence in Florence during his later years"
      },
      "geometry": {
        "type": "Point",
        "coordinates": [11.2526, 43.7676]
      }
    },
    {
      "type": "Feature",
      "properties": {
        "name": "Niccolò Machiavelli",
        "description": "Lived near Palazzo Vecchio, where he worked as a diplomat"
      },
      "geometry": {
        "type": "Point",
        "coordinates": [11.2556, 43.7678]
      }
    }
  ]
}
"""