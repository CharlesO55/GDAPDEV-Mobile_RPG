EXTERNAL SetNextStep(nNextStep, strQuestName)
EXTERNAL DoDialogueRoll(nStatRequired, strStatType)
EXTERNAL RewardItem(strItemName)

CONST _questName = "A Ritual Gone Wrong"
VAR _questStep = 0
VAR _rollSuccess = false

{_questStep :
- 0: -> Step0
- 1: -> Step1
- 2: -> Step2
}


===Step0===
As you're walking, a peculiar house appears. #SPEAKER:Narrator
Its windows are blasting rays of blinding light.
You swear you can hear CaramelDansen playing from inside.
You walk past to see a man standing outside the house.
Hey, you. #SPEAKER:Man
I need some help over here.
My house was overran by demons.
Luckily I managed to escape just in time and lock the door.
+ [Sounds scary man]
+ [Burn it down then]
- Are you crazy!?
That's my home you're talking about.
+ [The Demon King]
+ [Provides insurance claims]
- Do you take me for a conman?
I'll have you know that I'm Vergil the Great. #SPEAKER:Vergil
One of the previous heros who overthrew the Demon King.
+ [You can handle it then]
+ [Good luck]
- Ah wait. Wait!
I could still use your help.
+[Help]
Great, I knew I could count on you.
Listen, here's the key.
~RewardItem("Vergil House Key")
You go in the front door and start morbing all over the place.
And I'll umm...
I'll take the back door.
Got it?
~SetNextStep(1, _questName)
Great! Now get rid of those demons.
->END
+[Leave]
~ SetNextStep(-1, _questName) 
You take your leave as the demons turn this house into a moshpit. #SPEAKER:Narrator
->END


===Step1===
You succesfully enter the house and find the demons have made a mess of the place. #SPEAKER:Narrator
The demons notice your entrance and briefly pause.
Hey, bro. You here for the party to? #SPEAKER:Demon
+[Yes]
->Party
+[Leave]
Oh, shame.
~SetNextStep(1, _questName)
Feel free to crash here when you need a place.
->END

===Party===
Nice. #SPEAKER:Demon
Drinks are to the left and pizza is to the right.
Vergil is the bomb when it comes to these parties!
+[Vergil planned this?]
- Ya, see those summoning circles.
Dude scribbled all over the place and filled it with enough mana to summon an army.
+[Why'd he summon]
+[Demons]
- Don't ask me bro. I'm not the Legendary Necromancer of the Crypt.
If he wanted to party with us demons, he could've just set fliers all over the city.
Every demon in town couldn't resist clubbing.
Even the incubbi waiters would take a day off from Femboy Hooters.
Anyways, you do you.
Party's on full blast and those summoning circles will keep bringing in more guests.
Ciao, bro.
~SetNextStep(2, _questName)
We should probably stop those summoning circles. #SPEAKER:Narrator
->END

===Step2===
You begin investigating the summoning circle responsible for this. #SPEAKER:Narrator
Before you can finish, it suddenly glows immensely.
You step back to avoid being caught in it.
Everybody, run! #SPEAKER:Demons
The Key demon is here! 
A demonic arm soon emerges from the portal. #SPEAKER:Narrator
Slowly it drags drags itself out.
+[What's the key demon?]
-It's the demonic party pooper! #SPEAKER:Demon
Like a vampire that sucks all your rizz!
Whatever this demon is it seems to be a powerful opponent.#SPEAKER:Narrator
More than that it has a key on its neck.
It may be prove useful to take it.
RIZZLER! Where is it!?#SPEAKER:Key Demon
+[Fight]
-The demon howls an unholy screech as the battle begins.#SPEAEKER:Narrator
~RewardItem("Keep Gate Key")
~SetNextStep(3, _questName)
Let's defeat the Key demon.
->END