# The City In The Vale

This is a game done for the 3st Game Design Club challenge (Text Only) at the Universidade Lusófona Game Development degree.

The player explores a world using normal 3rd person view, with a rendering effect to seem to be made from text, and has to interact with the environment and objects with a text interface.


## Tech stuff

This game had several challenges, mainly because of the lack of time to build it.
The first experiment was done using a shader that converted a rendered scene into a "pseudo-ascii" view, based on this shader: https://www.shadertoy.com/view/lssGDj.
It didn't work as well as I wanted it to with the type of scene I had, so I changed it and used a lookup texture instead of predefined characters.
It worked for most of the development, but at the end I decided to go to a less technical shader, which I think it ended looking better. That shader worked by using a very long line of text that got offset and moved in lines to mask the underlying color (again, using a low-res render target).
I think it ended up working better for the intended game mood. You can check the different versions in game by using `set asciimode 0` to disable the effect, `set asciimode 1` to use the old shader, and `set asciimode 2` for the default version.
From the perspective of the adventure itself, I ended up quite satisfied with the results from a tech perspective.
Every interactable object contains a GameItem scriptable object, that describes the more "normal" properties (look at, name, etc). 
Then, you can associate any GameObject with an InteractableObject script and a Collider to get some basic behaviours. When you want the object to react to some verb, you just have to add one of the GameAction scripts and configure it. You can add more generic stuff (like PickupAction), and more specific behaviours (i.e. "CutTree").

The system could be taken a bit further with a better condition system and a more powerful action system (probably would be a great place to use a visual scripting tool).

Overall, I'm pretty happy with the game system as is, and I think this type of gameplay experience could be great, if the art and story was tailored to it.

## Credits

* Code, art, game design done by Diogo de Andrade

* Models
  * Trees: Pixel Processor (Stylized Tree Pack - Asset Store)
  * Rocks: Broken Vector (Lowpoly RTock/Treepack - Asset Store)

* Sfx
  * Whispers: Taure (https://freesound.org/people/taure/)
  * Campfire: CaganCelik (https://freesound.org/people/CaganCelik/)
  * Book: Tats14 (https://freesound.org/people/Tats14/)
  * Chest: Mafon2 (https://freesound.org/people/Mafon2/)
  * Tree: ecfike (https://freesound.org/people/ecfike/)
  * Lava: Fission9 (https://freesound.org/people/Fission9/)
  * Wind: lextrack(https://freesound.org/people/lextrack/)
  * Axe: jorickhoofd (https://freesound.org/people/jorickhoofd/)
  * Lighter: SomeoneCool15 (https://freesound.org/people/SomeoneCool15/)
  * Cloth: Daniel Poggioli (https://freesound.org/people/Dpoggioli/)
  * Creature: MinigunFiend (https://freesound.org/people/MinigunFiend/)

## Licenses

All code in this repo is made available through the [GPLv3] license.
The text and all the other files are made available through the 
[CC BY-NC-SA 4.0] license.

## Metadata

* Autor: [Diogo Andrade][]

[Diogo Andrade]:https://github.com/DiogoDeAndrade
[GPLv3]:https://www.gnu.org/licenses/gpl-3.0.en.html
[CC BY-NC-SA 4.0]:https://creativecommons.org/licenses/by-nc-sa/4.0/
[Bfxr]:https://www.bfxr.net/
