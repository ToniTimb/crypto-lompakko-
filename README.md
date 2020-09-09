# cryptot_sovellus



Tämä ohjalma on ensimmäinen c# kokeiluni, joten bugeja voi tulla vastaan. 
Tämän ei ole tarkoituskaan olla täydellinen vaan sisältää erilaisia kokeiluja kuinka asiat c# osalta voidaan hoitaa.

Kyseessä on siis konsoli sovellus Crypto "lompakko" jolla voi seurata https://www.coingecko.com/ tietojen pohjalta BTC, LTC ja ETH cryptojen arvon muutoksen vaikutusta omaan lompakkoo.

Ohjelma tallentaa lopetettaessa tiedon sisällöstä paikallisesti .json muodossa projektin juuressa olevaan Tiedostot kansioon ja sieltä myös lataa tiedot käynnistettäessä.

Lompakossa arvoa tarkistettaessa tehdään raportin tallennus projektin juuressa olevaan kansioon Tiedostot/raportit, kyseinen raportti sisältää tekstitiedostona lompakon tiedot kyseisellä hetkellä. 
Tilanteessa jossa nettiyhteys on poikki ohjelma käyttää viimeisintä siihen tallentunutta tietoa LTC,BTC,ETH arvoista ja ei lisää tästä uutta raporttia. 

Tärkeää huomata että windows ja osx käsittelevät tiedosto polkuja eritavalla joten koodiin on tarpeen tehdä muutokset sen mukaan kummassa järjestelmässä sitä ajetaan.

windows - osx muutokset

Kayttoliittyma.cs
rivit: 18-22

WINDOWS: 
private static string tallennus = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\Tiedostot\\";

OSX:
private static string tallennus = Directory.GetCurrentDirectory() + "/Tiedostot/";

rivit: 239-242

WINDOWS:
string tiedostoNimi = tallennus + "\\raportit\\"+ nyt.ToString() + ".txt";

OSX:
string tiedostoNimi = tallennus + "/raportit/"+ nyt.ToString() + ".txt";

TiedotApi.cs

rivi 16 

WINDOWS:
private static string historia = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\Tiedostot\\historia.json";

OSX:
private static string historia = Directory.GetCurrentDirectory() + "/Tiedostot/historia.json";
