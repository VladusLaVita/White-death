local door = script.Parent -- Сама дверь
local clickDetector = door:WaitForChild("ClickDetector")

local isOpen = false -- А дверь-то закрыта или нет?

local openRotation = 90 -- На сколько градусов повернуть
local closedRotation = 0

clickDetector.MouseClick:Connect(function()
	if not isOpen then
        for i = 0, openRotation do
            door.CFrame = door.CFrame * CFrame.Angles(0, math.rad(i), 0)
            wait(0.001)
        end
        isOpen = true
	else
        for i = openRotation, 0, -1 do
            door.CFrame = door.CFrame * CFrame.Angles(0, math.rad(-i), 0)
            wait(0.001)
        end
        isOpen = false
	end
end)