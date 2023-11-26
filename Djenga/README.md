
## Table of contents:
- [Introduction](#intro)
- [Technologies](#tech)
- [project Setup](#projo)
- [Illustrations](#illus)
- [Project Information](#info)
- [Contributing](#contri)
- [Acknowledgments](#know)

<INTRODUCTION>

<h1 id="intro">Djenga</h1>



Djenga is born out of the context of building without inference of materials being used. Hypothetically, The enclosing building elements such as floor, roof and walls are devoid of Material context when designing using BIM tools.
As a result the decison of which materials to be used are inferred late in the project and thus little impact during the design phase.

What Djenga seeks to achieve is to provide Building material data in realtime during the design phase. By a click of an element all encopassing material elements can be abstracted in realtime. 
As a result Buildings can be optimized better as earlier as during the design phase and thus achieve SDG 12 goal of responsible consumption and Production.

**usage**

The programme is able to intuitively :
1. Interact with RevitAPI and .NET frameworks
2. Abstract element data
3. Abstract material data 


<TECHNOLOGIES>

<h1 id="tech">Technologies</h1>

**Builth With**
- WPF
- RevitAPI
- c#


<PROJECT-SETUP>

<h1 id="projo">Project Setup</h1>


## Hardware Requirements
- You will need a desktop or a laptop computer.
- RAM: A minimum of 4GB RAM is recommended.
- Disk Space: You should have at least 5GB free of space on your working hard drive.

## Software Requirements

- Install Revit in your PC/desktop
- dll/executable file will be provided


<PROJECT-INFORMATION>

<h1 id="info">Project Information</h1>

**Project Status**


Iteration 01 -  Wall Element - Masonry Construction 
______________________________________________________________________________________________________________________________________
1. Allow the user to select a wall, multiple wall or all walls in a revit projects
2. Abstract the area, thickness, perimeter, and volume of the wall element from the Revit Model using revitAPI
3. Allow the user to input the type of block being used: Machine cut, Foundation Stone, Bricks and the mortar thickness
4. Calculate the stones needed, the amount of sand and cement needed for the mortar
5. Display the materials quantities needed for construction and their equivalent quantties
6. Share data: project data: project_name, project_unit_area; element_data: element-category, element_type, and  material_data:  ( Note that the type of element influences the materials to be used)
7. An add-in/plug-in with the following features:
    1. Element  choice- i.e User selects an element for analysis i.e Floor
    2. Element Type - User specifies element type i.e concrete ground floor slab
    3. Element Abstract -Generates a list for all materials required, with their quantities and their units of measure i.e cement, the volume of cement, bags of cement
    4. Element Export - Export as pdf or Excel format for pricing , quotation and sharing; write the list into Excel with Panda

**features**
- n/A

<CONTRIBUTING>

<h1 id="contri">🤝 Contributing</h1>

Contributions, issues and feature requests are always welcome!

I love meeting other developers, interacting and sharing.

Feel free to check the [issues page](https://github.com/symonkipkemei/Djenga/issues).

**How to Contribute**

To get a local copy up and running follow these simple example steps.

```
- Fork the repository
- git clone https://github.com/your_username/Djenga
- git checkout develop
- git checkout -b branch name
- git remote add upstream https://github.com/symonkipkemei/Djenga
- git pull upstream develop
- git commit -m "commit message"
- git push -u origin HEAD
```


<ACKNOWLEDGMENTS>

<h1 id="know">Acknowledgements</h1>

## Author

👤 **Symon Kipkemei**

- Github: [symonkipkemei](https://github.com/symonkipkemei)
- Twitter: [@symon_kipkemei](https://twitter.com/symon_kipkemei)
- LinkedIn: [Symon kipkemei](https://www.linkedin.com/in/symon-kipkemei/)


## Show your support


Finally, if you've read this far, don't forget to give this repo a ⭐️. 


## Acknowledgments

- [Kampaplays](https://www.youtube.com/@KampaPlays).
- [Programming with Mosh: Model-View-ViewModel](https://www.youtube.com/watch?v=fo6rvTP9kkc&t=311s)
- [Microsoft Learn](https://learn.microsoft.com/en-us/training/)
- [Revit API Developers Guide](https://help.autodesk.com/view/RVT/2023/ENU/?guid=Revit_API_Revit_API_Developers_Guide_html)
