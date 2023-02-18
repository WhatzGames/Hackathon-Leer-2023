using JaKleingartenParadies.Game;

const string SecretJaTastic = "2d376eb7-ead4-4b7c-99c0-3a21515e8cd5";
const string SecretJaTasticInvers = "f174f270-0271-4076-b314-25e6df884b3b";
const string SecretJaTestical1 = "74c3eaa1-afd4-461d-94b1-e17322b484ab";
const string SecretJaTestical2 = "1339ed02-36ba-45c0-95da-b777ee8f5cb6";

Bot JaTastic = new JaTasticBot(SecretJaTastic);
Bot JaTasticInvers = new JaTasticInversBot(SecretJaTasticInvers);
// Bot testical1 = new JaTasticBot(SecretJaTestical1);
// Bot testical2 = new JaTasticBot(SecretJaTestical2);

_ = JaTastic.Start();
_ = JaTasticInvers.Start();
// _ = testical1.Start();
// _ = testical2.Start();
    
while (true)
{
    Console.WriteLine("Still alive :) ");
    await Task.Delay(20000);
}