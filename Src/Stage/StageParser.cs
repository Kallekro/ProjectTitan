using System;
using System.Collections.Generic;
using System.IO;

namespace ProjectTitan
{
    public class StageParser
    {
        int m_road_width;
        string m_content_path;

        public StageParser(int road_width)
        {
            m_content_path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "../../../../Content/";
            m_road_width = 50;
        }

        public List<SegmentType>[] ParseStages()
        {
            List<SegmentType>[] result_stages = new List<SegmentType>[1];
            result_stages[0] = new List<SegmentType>();

            using (StreamReader file = new StreamReader(m_content_path + "stage1.txt"))
            {
                string line;
                int linecount = 1;
                while ((line = file.ReadLine()) != null)
                {
                    if (line.StartsWith("#", StringComparison.CurrentCulture))
                    {
                        continue;
                    }
                    string[] data = line.Split(' ');
                    int length = int.Parse(data[0]);
                    int degree = int.Parse(data[1]);

                    float n_segments_float = length / m_road_width;
                    int n_segments = 0;
                    if ((n_segments = (int)n_segments_float) != (int)Math.Round(n_segments_float))
                    {
                        Console.WriteLine("StageParser: Length of road stretch on line " + linecount + " is not divisible by segment width.");
                        continue;
                    }
                    SegmentType segment_type;

                    switch (degree)
                    {
                        case 0:
                            segment_type = SegmentType.Flat;
                            break;
                        case 5:
                            segment_type = SegmentType.Up5;
                            break;
                        case 10:
                            segment_type = SegmentType.Up10;
                            break;
                        case -5:
                            segment_type = SegmentType.Down5;
                            break;
                        case -10:
                            segment_type = SegmentType.Down10;
                            break;
                        default:
                            Console.WriteLine("StageParser: Degree of road stretch on line " + linecount + " is not defined.");
                            continue;
                    }

                    for (int i = 0; i < n_segments; i++)
                    {
                        result_stages[0].Add(segment_type);
                    }
                    linecount++;
                }
            }

            return result_stages;
        }
    }
}
