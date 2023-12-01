EXTERNAL SetNextStep(nNextStep, strQuestName)
EXTERNAL DoDialogueRoll(nStatRequired, strStatType)
EXTERNAL RewardItem(strItemName)
EXTERNAL StartBattle()
EXTERNAL AddMorality(nChange)


CONST _questName = "The Great Demonic Battlefront"
VAR _questStep = 0
VAR _rollSuccess = false

{_questStep :
- 0: -> Step0
- 1: -> Step1
- 2: -> Step2
- 3: -> Step3
- 4: -> Step4
- 5: -> Step5
- 6: -> Step6
- 7: -> Step7
- 8: -> Step8
}


===Step0===
4 brave travellers enter the town of Holden... #SPEAKER:Narrator
The last of the Demon King's strongholds.
One by one, your party has toppled the Demon army's generals.
This will be our final battle.
Yet it will be difficult to win alone.
Perhaps the townspeople would be willing to support humanity's greatest triumph.
+[Indeed.]
~ SetNextStep(1, _questName)
A famed shopkeeper is said to have sheltered the legendary weapon.
Approaching them would be your best move.
-> END

===Step1===
You find what seems to be a shop. #SPEAKER:Narrator
There's various weapons and items strewn through out the place.
+[Is this a shop?]
-What do you think, genius? #SPEAKER:Man
A man approaches you. #SPEAKER:Narrator
Yes, I'm a man. #SPEAKER:Man
And the name's Rhein. #SPEAKER:Rhein
We're swordsmiths.
What's got you crawling into a place like this?
+[The Demon King..]
+[Must be toppled]
-Kid, I don't care what war you're fighting.
All I care is what you're paying with.
Don't tell me you came all this way expecting us to give you a free blade.
+[But wait!]
Scram, kid.
I don't do charity cases.
Well, that was rude. #SPEAKER:Narrator
~ SetNextStep(2, _questName)
Still, there may be more someone else who can assist us.
-> END

===Step2===
You see a man forging a bizarre weapon.
Woah, keep your distance now. #SPEAKER:Weaponsmith
She gets quite the temper when you get too close.
You wait for the blacksmith to finish their work. #SPEAKER:Narrator
Soon he douses his craft in some water.
So how can I help you, kid? #SPEAKER:Weaponsmith
+ [I need a weapon]
-Well, that's what everyone's here for.
Tell me what it's for.
+ [The Demon King]
-Oh now. We're talking!
We could go big!
Claymores forged from dwarven titanium.
Or go stealthy with some darts coated in cocanium.
Or perhaps you're a classy fella?
A classic rapier.
No. No. No. That toothpick won't be enough.
The Demon King could take a blow or two.
The weaponsmith continues muttering to himself #SPEAKER:Narrator
Perhaps taking his best work would suffice.
+[Let's see your best]
+[Any weapon you have]
-Ah, sorry kid. #SPEAKER:Weaponsmith
Rhein's already got those preordered by the Demon King's Army.
+[What about those swords in the corner?]
-Oh, those are already ordered by the rebels.
+[But we are the rebels!]
-Nope. You got it wrong.
Those swords are for the demons who'll be rebelling after this Demon King uprising.
Then those axes will be for the humans who'll rebel after the 5th demonic rebellion.
We even saved up some sticks and stones up for when the Demonic nuclear war takes place.
+[But this is the]
+[The Great Demonic Battle]
-That's just a quest title, kid.
Me and my wife also call our nightly tussles The Great Bed Wars.
But don't feel bad.
I'll try to cook something up just for you.
~ SetNextStep(3, _questName)
Come back later kid.
->END

=== Step3 ===
Alright, welcome back kid. #SPEAKER:Weaponsmith
I made this one behind Rhein's back.
Weaponsmith's like us don't usually bypass the law but if it can remove gun regulations, I'm all in.
+[Wait, guns?]
- Yea, that's right.
Look at this puppy!
THUMP. He drops a massive chaingun on the floor. #SPEAKER:Narrator
Perhaps, these weaponsmiths were not best idea.
Hohoho, 6 barrels with 1000 rounds. #SPEAKER:Weaponsmith
Firing this weapon for 12 seconds cost 400,000 dollars.
+[I can't afford that]
-It's on the house.
+[I can't carry that]
-But you can swing a big ass claymore?
+[It breaks my immersion]
-Mate, the game is made from random assets the devs found.
Listen kid. I went through all this effort and you're just gonna bail?
->AngryWeaponsmith

=== AngryWeaponsmith ===
Bruh... #SPEAKER:Narrator

* [Rebute CON(8)]
    ->Rebute
* [Convince CHA(8)]
    ->Convince
+ [Run]
    ~ SetNextStep(2, _questName)
    ~ AddMorality(-3)
    You dash out the door as Rhein and his brother scream at you.
    Guards saw you dashing out the building and grow suspicious.
    The guards soon investigate and confiscate the proclaimed 6 piped chandelier.
    Rhein Metalworks may face eviction upon the investigation's conclusion.
    -> END
    
    
=== Rebute ===
There's no way you can use a chaingun. #SPEAKER:Narrator
A quarrel soon breaks out.
    +[I asked for a normal weapon]
    +[I didn't tell you]
    +[to go the extra mile]
-His face stiffens with your ungrateful attitude. #SPEAKER:Narrator
Each word you hurl at him continues to shatter his dream.
~ DoDialogueRoll(8, "CON")
His face fumes red with anger as he prepares to berate you.
{ _rollSuccess :
    [{_rollSuccess}] Insults are hurled at you. 
        Yet you don't flinch and walk out the door.
        ~ SetNextStep(4, _questName)
        Even without a weapon, you should still be able to enter the castle.
        ->END
        -else:
        [{_rollSuccess}] Each insult tears at your heart.
        ~ AddMorality(-1)
        Soon you begin to feel the guilt and reconsider.
        ->AngryWeaponsmith
}

=== Convince ===
There's no way you can use a chaingun. #SPEAKER:Narrator
You try your best to convince them with empty promises.
~ DoDialogueRoll(8, "CHA")
Yet they remain dubious.
{_rollSuccess:
    [{_rollSuccess}] You promise that you'll pick it up by lunch. #SPEAKER:Narrator
        All right then. #SPEAKER:Weaponsmith
        You're permitted to leave the store.#SPEAKER:Narrator
        ~ SetNextStep(4, _questName)
        We should head for the castle before they find out.
        ->END
    -else:
    [{_rollSuccess}] Kid, just come out straight. #SPEAKER:Weaponsmith
        I know a liar when I see one.
        ~AddMorality(-1)
        My brother may be a dick, but at least he's real with me.
        ->AngryWeaponsmith
}

    
=== Step4 ===
You approach the castle door.#SPEAKER:Narrator
It seems difficult to enter but with the right skills, it should budge.
*[Pick lock DEX(12)]
    ~ DoDialogueRoll(12, "DEX")
    You bring out your lockpick and begin working.
    {_rollSuccess:
        [{_rollSuccess}] A click is heard and the lock falls off.
        ~ SetNextStep(6, _questName)
        ~ RewardItem("Keep Gate Key")
        Only the Demon King remains.
        ->END
        - else:
        [{_rollSuccess}] Your random jabs and thrusts prove no match.
        ~AddMorality(-1)
        The lock is victorious against your unskillful hands.
        ->Step4
    }
*[Break door STR(12)]
    ~ DoDialogueRoll(12, "STR")
    You muster all your strength to kick the door down.
    {_rollSuccess:
        [{_rollSuccess}] The door goes flying as it's smashed apart.
        ~ SetNextStep(6, _questName)
        ~ RewardItem("Keep Gate Key")
        Only the Demon King remains.
        ->END
        - else:
        [{_rollSuccess}] You kick the door over and over again yet it still stands.
        The door's vision slit opens revealing the face of a chubby guard.
        Hehe, thanks for the massage! #SPEAKER:Guard
        ~AddMorality(-1)
        Can you kick just a little more to the left?
        ->Step4
    }
*[Get key]
    Behind the door, you hear the guards talking.
    According to them, the key was lost in the abandoned house.
    ~ SetNextStep(5, _questName)
    We should go look for it.
->END

===  Step5 ===
You pick up the key. #SPEAKER:Narrator
~ SetNextStep(6, _questName)
~ RewardItem("Keep Gate Key")
Huh, it was just right here all along?
Finally, we can enter the castle gate.
->END

=== Step6 ===
You enter the castle and prepare to fight the Demon King.#SPEAKER:Narrator
~ SetNextStep(7, _questName)
We must navigate the long and twisting corridors to the throne room.
->END

=== Step7 ===
The Demon King is busy with work that they don't notice your presence #SPEAKER:Narrator
Now could be the perfect oppurtunity to strike.
+ [Attack]
~ SetNextStep(8, _questName)
You charge forward and begin your attack.
~ StartBattle()
->END

=== Step8 ===
The Demon King is slain. #SPEAKER:Narrator
With that, humanity is once again free to determine it's own fate.
->END