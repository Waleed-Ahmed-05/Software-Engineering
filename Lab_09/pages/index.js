import path from 'path';
import fs from 'fs';
import matter from 'gray-matter';

export default function Home({ postData }) {
    return (
        <div>
            <h1>{postData.data.title}</h1>
            <p>{postData.data.date}</p>
            <div dangerouslySetInnerHTML={{ __html: postData.content }} />
        </div>
    );
}

export async function getStaticProps() {
    const filePath = path.join(process.cwd(), 'posts', 'hello-world.md');
    const fileContents = fs.readFileSync(filePath, 'utf8');
    const { data, content } = matter(fileContents);

    return {
        props: {
            postData: { data, content },
        },
    };
}
