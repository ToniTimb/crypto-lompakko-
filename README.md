# crypto-lompakko



Tämä ohjalma on ensimmäinen c# kokeiluni, joten bugeja voi tulla vastaan. 
Tämän ei ole tarkoituskaan olla täydellinen vaan sisältää erilaisia kokeiluja kuinka asiat c# osalta voidaan hoitaa.

Kyseessä on siis konsoli sovellus Crypto "lompakko" jolla voi seurata https://www.coingecko.com/ tietojen pohjalta BTC, LTC ja ETH cryptojen arvon muutoksen vaikutusta omaan lompakkoo.

Ohjelma tallentaa lopetettaessa tiedon sisällöstä paikallisesti .json muodossa projektin juuressa olevaan Tiedostot kansioon ja sieltä myös lataa tiedot käynnistettäessä. Jos kyseistä kansiota ei löydy niin sovellus lisää sen tarvittaessa.

Lompakossa arvoa tarkistettaessa tehdään raportin tallennus projektin juuressa olevaan kansioon Tiedostot/raportit, kyseinen raportti sisältää tekstitiedostona lompakon tiedot kyseisellä hetkellä. 

Tilanteessa jossa nettiyhteys on poikki ohjelma käyttää viimeisintä siihen tallentunutta tietoa LTC,BTC,ETH arvoista ja ei lisää tästä uutta raporttia. 

