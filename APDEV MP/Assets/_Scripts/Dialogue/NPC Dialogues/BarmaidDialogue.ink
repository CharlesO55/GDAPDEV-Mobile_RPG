EXTERNAL ChangeStat(strStat, nAmount, bEntireParty)
EXTERNAL DoQuickRoll(nPassingVal)

VAR _rollSuccess = false

Hey, hero.#SPEAKER:Barmaid
You look worse for wear.
Would like a drink?
+[Sure]
->Drink
+[No, thanks]
Ah, bummer.
->END

===Drink===
Great! Our drinks are to die for.
~ DoQuickRoll(3)
Here you go.
{ _rollSuccess:
    You take a sip and its sweet.#SPEAKER:Narrator
    Your body feels rejunevated.
    ~ ChangeStat("HP", 2, true)
    Glad you liked it.#SPEAKER:Narrator
    Make sure to come again.
    ->END
    -else:
    You take a sip and it tastes bitter af.#SPEAKER:Narrartor
    Your vision blurs and your hearing becomes murked.
    I'mmm GLasd yUU likeeed iTT.#SPEAKER:Barmaid
    TeeLLLL miii urrrRR LiffWE INSurrance pOLicE NuMBA pwease...
    ~ ChangeStat("HP", -100, true)
    Oh no!
}