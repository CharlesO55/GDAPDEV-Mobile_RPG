EXTERNAL ChangeStat(strStat, nAmount, bEntireParty)
EXTERNAL DoQuickRoll(nPassingVal)

VAR _rollSuccess = false

Hey, hero.#SPEAKER:Barmaid
~ DoQuickRoll(15)
You look worse for wear.
{_rollSuccess:
    ->Medicine
    -else:
    ->Poison
}
->END


===Medicine===
Have a drink.
+[Cheers!]
    You take a sip and its sweet.#SPEAKER:Narrator
    Your body feels rejunevated.
    ~ ChangeStat("HP", 6, true)
    Make sure to come again..#SPEAKER:Barmaid
    ->END
+[No, thanks]
    Suit yourself.
    ->END


===Poison===
Have a drink.
It's to die for.
+[Cheers!]
    You take a sip and it tastes bitter af.#SPEAKER:Narrartor
    I'mmm GLasd yUU likeeed iTT.#SPEAKER:Barmaid
    Your vision blurs and your hearing becomes murked.#SPEAKER:Narrator
    You shouldn't have drank.
    This game is only rated 12.
    TeeLLLL miii urrrRR LiffWE INSurrance pOLicE NuMBA pwease...#SPEAKER:Barmaid
    Luckily, poison is rated for all ages.#SPEAKER:Narrator
    Oh no!#SPEAKER:Barmaid
    ~ ChangeStat("HP", -100, true)
    ->END
+[No, thanks]
    Suit yourself.
    ->END