/* 
* This software is provided 'as-is', without any express or implied
* warranty. In no event will the authors be held liable for any damages
* arising from the use of this software.
* 
* Permission is granted to anyone to use this software for any purpose,
* including commercial applications, and to alter it and redistribute it
* freely, subject to the following restrictions:
* 
* 1. The origin of this software must not be misrepresented; you must not
* claim that you wrote the original software. If you use this software
* in a product, an acknowledgment in the product documentation would be
* appreciated but is not required.
* 
* 2. Altered source versions must be plainly marked as such, and must not be
* misrepresented as being the original software.
* 
* 3. This notice may not be removed or altered from any source distribution.
*/


#include "stdafx.h"
#include "Newton.h"
#include "dNewtonBody.h"
#include "dNewtonWorld.h"
#include "dNewtonCollisionBox.h"

//dNewtonCollisionBox::dNewtonCollisionBox(dNewtonWorld* const world, dFloat x, dFloat y, dFloat z, dLong collisionMask)
dNewtonCollisionBox::dNewtonCollisionBox(dNewtonWorld* const world, dFloat x, dFloat y, dFloat z)
//	:dNewtonCollision(m_box, collisionMask)
	:dNewtonCollision(m_box, 0)
{
	dAssert(0);
	SetShape(NewtonCreateBox(world->m_world, x, y, z, 0, NULL));
}

/*
dNewtonCollisionBox::dNewtonCollisionBox(const dNewtonCollisionBox& srcCollision, NewtonCollision* const shape)
	: dNewtonCollision(srcCollision, shape)
{
}


dNewtonCollision* dNewtonCollisionBox::Clone(NewtonCollision* const shape) const
{
	return new dNewtonCollisionBox(*this, shape);
}
*/