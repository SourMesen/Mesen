/* This is a heavily modified version of the file used in RetroArch */

/*  RetroArch - A frontend for libretro.
 *  Copyright (C) 2010-2014 - Hans-Kristian Arntzen
 *  Copyright (C) 2011-2014 - Daniel De Matteis
 *
 *  RetroArch is free software: you can redistribute it and/or modify it under the terms
 *  of the GNU General Public License as published by the Free Software Found-
 *  ation, either version 3 of the License, or (at your option) any later version.
 *
 *  RetroArch is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
 *  PURPOSE.  See the GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License along with RetroArch.
 *  If not, see <http://www.gnu.org/licenses/>.
 */

#ifdef _WIN32
#include "stdafx.h"
#endif
#include "../stdafx.h"

#define twoxsai_interpolate_xrgb8888(A, B) ((((A) & 0xFEFEFEFE) >> 1) + (((B) & 0xFEFEFEFE) >> 1) + ((A) & (B) & 0x01010101))

#define twoxsai_interpolate2_xrgb8888(A, B, C, D) ((((A) & 0xFCFCFCFC) >> 2) + (((B) & 0xFCFCFCFC) >> 2) + (((C) & 0xFCFCFCFC) >> 2) + (((D) & 0xFCFCFCFC) >> 2) + (((((A) & 0x03030303) + ((B) & 0x03030303) + ((C) & 0x03030303) + ((D) & 0x03030303)) >> 2) & 0x03030303))

#define twoxsai_interpolate_rgb565(A, B) ((((A) & 0xF7DE) >> 1) + (((B) & 0xF7DE) >> 1) + ((A) & (B) & 0x0821))

#define twoxsai_interpolate2_rgb565(A, B, C, D) ((((A) & 0xE79C) >> 2) + (((B) & 0xE79C) >> 2) + (((C) & 0xE79C) >> 2) + (((D) & 0xE79C) >> 2)  + (((((A) & 0x1863) + ((B) & 0x1863) + ((C) & 0x1863) + ((D) & 0x1863)) >> 2) & 0x1863))

#define twoxsai_interpolate_4444(A, B) (((A & 0xEEEE) >> 1) + ((B & 0xEEEE) >> 1) + (A & B & 0x1111))
#define twoxsai_interpolate2_4444(A, B, C, D) (((A & 0xCCCC) >> 2) + ((B & 0xCCCC) >> 2) + ((C & 0xCCCC) >> 2) + ((D & 0xCCCC) >> 2) + ((((A & 0x3333) + (B & 0x3333) + (C & 0x3333) + (D & 0x3333)) >> 2) & 0x3333))

#define twoxsai_result(A, B, C, D) (((A) != (C) || (A) != (D)) - ((B) != (C) || (B) != (D)));

#define twoxsai_declare_variables(typename_t, in, nextline) \
         typename_t product, product1, product2; \
         typename_t colorI = *(in - nextline - 1); \
         typename_t colorE = *(in - nextline + 0); \
         typename_t colorF = *(in - nextline + 1); \
         typename_t colorJ = *(in - nextline + 2); \
         typename_t colorG = *(in - 1); \
         typename_t colorA = *(in + 0); \
         typename_t colorB = *(in + 1); \
         typename_t colorK = *(in + 2); \
         typename_t colorH = *(in + nextline - 1); \
         typename_t colorC = *(in + nextline + 0); \
         typename_t colorD = *(in + nextline + 1); \
         typename_t colorL = *(in + nextline + 2); \
         typename_t colorM = *(in + nextline + nextline - 1); \
         typename_t colorN = *(in + nextline + nextline + 0); \
         typename_t colorO = *(in + nextline + nextline + 1);

#ifndef twoxsai_function
#define twoxsai_function(result_cb, interpolate_cb, interpolate2_cb) \
         if (colorA == colorD && colorB != colorC) \
         { \
            if ((colorA == colorE && colorB == colorL) || (colorA == colorC && colorA == colorF && colorB != colorE && colorB == colorJ)) \
               product = colorA; \
            else \
            { \
               product = interpolate_cb(colorA, colorB); \
            } \
            if ((colorA == colorG && colorC == colorO) || (colorA == colorB && colorA == colorH && colorG != colorC && colorC == colorM)) \
               product1 = colorA; \
            else \
            { \
               product1 = interpolate_cb(colorA, colorC); \
            } \
            product2 = colorA; \
         } else if (colorB == colorC && colorA != colorD) \
         { \
            if ((colorB == colorF && colorA == colorH) || (colorB == colorE && colorB == colorD && colorA != colorF && colorA == colorI)) \
               product = colorB; \
            else \
            { \
               product = interpolate_cb(colorA, colorB); \
            } \
            if ((colorC == colorH && colorA == colorF) || (colorC == colorG && colorC == colorD && colorA != colorH && colorA == colorI)) \
               product1 = colorC; \
            else \
            { \
               product1 = interpolate_cb(colorA, colorC); \
            } \
            product2 = colorB; \
         } \
         else if (colorA == colorD && colorB == colorC) \
         { \
            if (colorA == colorB) \
            { \
               product  = colorA; \
               product1 = colorA; \
               product2 = colorA; \
            } \
            else \
            { \
               int r = 0; \
               product1 = interpolate_cb(colorA, colorC); \
               product  = interpolate_cb(colorA, colorB); \
               r += result_cb(colorA, colorB, colorG, colorE); \
               r += result_cb(colorB, colorA, colorK, colorF); \
               r += result_cb(colorB, colorA, colorH, colorN); \
               r += result_cb(colorA, colorB, colorL, colorO); \
               if (r > 0) \
                  product2 = colorA; \
               else if (r < 0) \
                  product2 = colorB; \
               else \
               { \
                  product2 = interpolate2_cb(colorA, colorB, colorC, colorD); \
               } \
            } \
         } \
         else \
         { \
            product2 = interpolate2_cb(colorA, colorB, colorC, colorD); \
            if (colorA == colorC && colorA == colorF && colorB != colorE && colorB == colorJ) \
               product = colorA; \
            else if (colorB == colorE && colorB == colorD && colorA != colorF && colorA == colorI) \
               product = colorB; \
            else \
            { \
               product = interpolate_cb(colorA, colorB); \
            } \
            if (colorA == colorB && colorA == colorH && colorG != colorC && colorC == colorM) \
               product1 = colorA; \
            else if (colorC == colorG && colorC == colorD && colorA != colorH && colorA == colorI) \
               product1 = colorC; \
            else \
            { \
               product1 = interpolate_cb(colorA, colorC); \
            } \
         } \
         out[0] = colorA; \
         out[1] = product; \
         out[dst_stride] = product1; \
         out[dst_stride + 1] = product2; \
         ++in; \
         out += 2
#endif

void twoxsai_generic_xrgb8888(unsigned width, unsigned height, uint32_t *src, unsigned src_stride, uint32_t *dst, unsigned dst_stride)
{
   unsigned finish;
	for(; height; height--) {
		uint32_t *in = (uint32_t*)src;
		uint32_t *out = (uint32_t*)dst;

		for(finish = width; finish; finish -= 1) {
			twoxsai_declare_variables(uint32_t, in, (height > 1 ? src_stride : 0));

			/*
			 * Map of the pixels:           I|E F|J
			 *                              G|A B|K
			 *                              H|C D|L
			 *                              M|N O|P
			 */

			twoxsai_function(twoxsai_result, twoxsai_interpolate_xrgb8888, twoxsai_interpolate2_xrgb8888);
		}

		src += src_stride;
		dst += 2 * dst_stride;
	}
}
