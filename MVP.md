
<h1 id="intro">Djenga</h1>

Djenga is born out of the context of building without inference of materials being used. Hypothetically, The enclosing building elements such as floor, roof and walls are devoid of Material context when designing using BIM tools.
As a result the decison of which materials to be used are inferred late in the project and thus little impact during the design phase.

What Djenga seeks to achieve to provide Building material data in realtime during the design phase. By a click of an element all encopassing material elements can be abstracted in realtime. 
As a result Buildings can be optimized better as earlier as during the design phase and thus achieve SDG 12 goal of responsible consumption and Production.


Project Link: https://github.com/symonkipkemei/Djenga.git



**Business and Market Needs**
______________________________________________________________________________________________________________________________________

- Architects /Builders /Designers and  Quantity Surveyors want real-time access to material data of their buildings for better control of the design and cost outcome.

Sustainable Development Goals
-  Sustainable Cities and Communities (SDG 11)
- Responsible Consumption and Production (SDG 12)

Long term goal
- Fully democratize access to real-time material data for buildings.
- Improve optimization of building materials for every built project by 50%


 Success Criteria
- If we can achieve 30% of building material optimization, and have 400 monthly users

Leverage
- Construction and building technology is decentralized, taps into and dominates the building material sector for East Africa



**Mapping Out User Journey**
______________________________________________________________________________________________________________________________________
Identify the user 

- The user: Architects, Designers, Quantity Surveyors, Contractors, Builders, master-builder
- The Modeler: Generates Revit models 

Identify the actions

- Architect - Designs and generate a model, Get material data, compare one type of slab to another, Optimize design to use more/less material
- Contractor- Determine how many material elements are needed to fabricate/construct a wall/slab etc
- Quantity Surveyor- Get a spreadsheet of all quantities of material in Excel format. To edit and fill up the appropriate rates
- Client /builder- Get a spreadsheet of all quantities of material  for bidding and pricing  by different suppliers/hardware



A pain and Gain Map
 
- Action: Get material Data
- Pains : Traditionally material data is provided after the design is complete and not during design stage , quantity surveyor does take off to determine quantities data from drawings , quantity surveryor calculates materials needed for each element.
- Gains. Get realtime material data from BIM models , Get accurate material estimates, Optimize material usage early in the design stage, 


**Features to build**
______________________________________________________________________________________________________________________________________

User Needs:
- Realtime Quantities of materials needed to construct an element; start with walls.....,start with a bungalow house

User wants:
1. Materials needed to construct different elements
2. Generate excel spreadsheet of all quantities of materials with an empty rates column.
3. Compare change of material quantities through the lifetime of the design (Every time get material data is requested, a previous version of the same is saved)
4. Generative design on how to optimize/install/place each material 



**Building Tools**
______________________________________________________________________________________________________________________________________

1. C#
2. RevitAPI
3. Autodesk Revit 2023
4. WPF ( Windows Presentation Form)


**Building Process - Iteration 01 -  Wall Element - Masonry Construction **
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